using System;
using System.Windows;
using System.Windows.Media;

namespace MiraAuxPanel
{
    public partial class MainWindow : Window
    {
        private SensitivityEngine _engine;
        private bool _isEngineActive = false;

        public MainWindow()
        {
            InitializeComponent();
            _engine = new SensitivityEngine();
        }

        // Atualiza o multiplicador X dinamicamente quando você mexe no Slider
        private void SliderX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_engine != null)
            {
                _engine.MultiplierX = e.NewValue;
            }
        }

        // Atualiza o multiplicador Y dinamicamente quando você mexe no Slider
        private void SliderY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_engine != null)
            {
                _engine.MultiplierY = e.NewValue;
            }
        }

        // Liga / Desliga o motor de sensibilidade
        private void BtnToggleEngine_Click(object sender, RoutedEventArgs e)
        {
            _isEngineActive = !_isEngineActive;

            if (_isEngineActive)
            {
                BtnToggleEngine.Content = "DESATIVAR MOTOR DE MIRA";
                BtnToggleEngine.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4500")); // Vermelho Laranja
                BtnToggleEngine.Foreground = Brushes.White;
                TxtEngineStatus.Text = $"Motor Ativo: X ({_engine.MultiplierX:F1}x) | Y ({_engine.MultiplierY:F1}x)";
                TxtEngineStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FF7F")); // Verde
                
                // Aqui no futuro interceptamos o mouse hook em baixo nível para recalcular os deltas X e Y
            }
            else
            {
                BtnToggleEngine.Content = "ATIVAR MOTOR DE MIRA";
                BtnToggleEngine.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FF7F")); // Verde
                BtnToggleEngine.Foreground = Brushes.Black;
                TxtEngineStatus.Text = "Motor de Sensibilidade: Inativo";
                TxtEngineStatus.Foreground = Brushes.Orange;
            }
        }

        // Executa as otimizações de registro
        private void BtnApplyTweaks_Click(object sender, RoutedEventArgs e)
        {
            // Aplica tweaks de remoção de aceleração residual
            bool mouseTweaks = RegistryTweaks.ApplyLowLatencyTweaks();
            
            // Aplica a otimização de tamanho da fila (Buffer)
            bool queueTweaks = RegistryTweaks.OptimizeMouseQueue();

            if (mouseTweaks && queueTweaks)
            {
                MessageBox.Show("Otimizações de Input aplicadas com sucesso!\n\nNota: Para o 'MouseDataQueueSize' surtir efeito total, reinicie o computador.", 
                                "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (mouseTweaks && !queueTweaks)
            {
                MessageBox.Show("Aceleração residual limpa!\n\nPorém, a otimização de buffer (Fila) falhou. Certifique-se de executar o painel como ADMINISTRADOR para modificar os drivers do sistema.", 
                                "Aviso de Privilégios", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Falha crítica ao tentar injetar otimizações no Registro do Windows.", 
                                "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}