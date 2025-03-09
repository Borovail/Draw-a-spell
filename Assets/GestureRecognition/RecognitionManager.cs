#nullable enable
using System;
using System.Linq;
using FreeDraw;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecognitionManager : MonoBehaviour
{
    [SerializeField] private Drawable _drawable = null!;
    [SerializeField] private TextMeshProUGUI _recognitionResult = null!;

    [SerializeField] private Button? _templateModeButton;
    [SerializeField] private Button? _recognitionModeButton;
    [SerializeField] private Button? _reviewTemplates;
    [SerializeField] private TMP_InputField? _templateName;
    [SerializeField] private TemplateReviewPanel? _templateReviewPanel;
    [SerializeField] private RecognitionPanel? _recognitionPanel;

    private GestureTemplates _templates => GestureTemplates.Get();
    private static readonly DollarOneRecognizer _dollarOneRecognizer = new DollarOneRecognizer();
    private static readonly DollarPRecognizer _dollarPRecognizer = new DollarPRecognizer();
    private IRecognizer _currentRecognizer = _dollarOneRecognizer;
    private RecognizerState _state = RecognizerState.RECOGNITION;

    public enum RecognizerState
    {
        TEMPLATE,
        RECOGNITION,
        TEMPLATE_REVIEW
    }

    [Serializable]
    public struct GestureTemplate
    {
        public string Name;
        public DollarPoint[] Points;

        public GestureTemplate(string templateName, DollarPoint[] preparePoints)
        {
            Name = templateName;
            Points = preparePoints;
        }
    }

    private string TemplateName => _templateName.text;


    private void Start()
    {
        _drawable.OnDrawFinished += OnDrawFinished;
        if (_templateModeButton != null)
        {
            _templateModeButton.onClick.AddListener(() => SetupState(RecognizerState.TEMPLATE));
            _recognitionModeButton.onClick.AddListener(() => SetupState(RecognizerState.RECOGNITION));
            _reviewTemplates.onClick.AddListener(() => SetupState(RecognizerState.TEMPLATE_REVIEW));
            _recognitionPanel.Initialize(SwitchRecognitionAlgorithm);
        }

        if (_templateModeButton == null)
            SwitchRecognitionAlgorithm(1);

        SetupState(_state);

    }

    private void SwitchRecognitionAlgorithm(int algorithm)
    {
        if (algorithm == 0)
        {
            _currentRecognizer = _dollarOneRecognizer;
        }

        if (algorithm == 1)
        {
            _currentRecognizer = _dollarPRecognizer;
        }
    }

    private void SetupState(RecognizerState state)
    {
        _state = state;
        if (_templateModeButton != null)
        {

            _templateModeButton.image.color = _state == RecognizerState.TEMPLATE ? Color.green : Color.white;
            _recognitionModeButton.image.color = _state == RecognizerState.RECOGNITION ? Color.green : Color.white;
            _reviewTemplates.image.color = _state == RecognizerState.TEMPLATE_REVIEW ? Color.green : Color.white;
            _templateName.gameObject.SetActive(_state == RecognizerState.TEMPLATE);
            _templateReviewPanel.SetVisibility(state == RecognizerState.TEMPLATE_REVIEW);
            _recognitionPanel.SetVisibility(state == RecognizerState.RECOGNITION);
        }

        _recognitionResult.gameObject.SetActive(_state == RecognizerState.RECOGNITION);

        _drawable.gameObject.SetActive(state != RecognizerState.TEMPLATE_REVIEW);
    }

    private void OnDrawFinished(DollarPoint[] points)
    {
        if (_state == RecognizerState.TEMPLATE)
        {
            GestureTemplate preparedTemplate =
                new GestureTemplate(TemplateName, _currentRecognizer.Normalize(points, 64));
            _templates.RawTemplates.Add(new GestureTemplate(TemplateName, points));
            _templates.ProceedTemplates.Add(preparedTemplate);
        }
        else
        {
            //  (string, float) result = _dollarOneRecognizer.DoRecognition(points, 64, _templates.GetTemplates());
            (string, float) result = _currentRecognizer.DoRecognition(points, 64,
                _templates.RawTemplates);
            string resultText = "";
            if (result.Item2 <= 0)
            {
                resultText = "No gesture recognized ):";
            }
            else if (_currentRecognizer is DollarOneRecognizer)
            {
                resultText = $"Recognized: {result.Item1}, Score: {result.Item2}";
            }
            else if (_currentRecognizer is DollarPRecognizer)
            {
                resultText = $"Recognized: {result.Item1}, Distance: {result.Item2}";
                if (ParticleManager.Instance != null) ParticleManager.Instance.SpawnParticle(result.Item1, result.Item2);
            }

            _recognitionResult.text = resultText;
        }
    }

    private void OnApplicationQuit()
    {
        _templates.Save();
    }
}