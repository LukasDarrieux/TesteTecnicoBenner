namespace SistemaPedidos.Functions
{
    public static class Utils
    {
        /// <summary>
        /// Permitir apenas números e vírgula
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DecimalPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != ",")
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Permitir apenas números
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void NumberPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Permitir apenas números e caracteres de formatação do CPF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CpfPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != "." && e.Text != "-" && e.Text != ",")
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Permitir apenas números e caracteres de formatação do CEP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CepPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != "-")
            {
                e.Handled = true;
            }
        }
    }
}
