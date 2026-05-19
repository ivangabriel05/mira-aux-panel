using System;
using Microsoft.Win32;

namespace MiraAuxPanel
{
    public static class RegistryTweaks
    {
        // Caminho do registro para configurações do mouse
        private const string MouseKeyPath = @"Control Panel\Desktop";

        /// <summary>
        /// Otimiza o buffer do mouse e remove a aceleração residual do Windows.
        /// </summary>
        public static bool ApplyLowLatencyTweaks()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(MouseKeyPath, true))
                {
                    if (key != null)
                    {
                        // Desativa a aceleração do mouse (Fix de precisão 1:1)
                        key.SetValue("MouseSpeed", "0", RegistryValueKind.String);
                        key.SetValue("MouseThreshold1", "0", RegistryValueKind.String);
                        key.SetValue("MouseThreshold2", "0", RegistryValueKind.String);
                        
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao aplicar tweaks de mouse: {ex.Message}");
            }
            return false;
        }

        /// <summary>
        /// Ajusta o tamanho da fila de dados do mouse para reduzir o Input Lag.
        /// Nota: Requer privilégios de Administrador.
        /// </summary>
        public static bool OptimizeMouseQueue()
        {
            try
            {
                // Caminho dos drivers de serviço do mouse no sistema
                string systemKeyPath = @"System\CurrentControlSet\Services\Mouclass\Parameters";
                
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(systemKeyPath, true))
                {
                    if (key != null)
                    {
                        // Diminui o tamanho do buffer para respostas mais rápidas (padrão costuma ser 100)
                        key.SetValue("MouseDataQueueSize", 20, RegistryValueKind.DWord);
                        return true;
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Erro: Execute o aplicativo como Administrador para modificar o buffer do sistema.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao otimizar fila do mouse: {ex.Message}");
            }
            return false;
        }
    }
}