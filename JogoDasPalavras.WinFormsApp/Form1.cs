namespace JogoDasPalavras.WinFormsApp
{
    public partial class Form1 : Form
    {

        private JogoDePalavras jogoPalavras;

        public Form1()
        {
            InitializeComponent();

            ConfigurarBotoes();

            ComecarJogo();
        }

        private void ConfigurarBotoes()
        {
            btnEnter.Click += btnEnter_Click;
            lblNovoJogo.Click += NovoJogo;

            foreach (Button botao in pnlTeclas.Controls)
            {
                if (botao.Text != "Enter")
                {
                    botao.Click += DarPalpite;
                }
            }
        }

        private void ComecarJogo()
        {
            jogoPalavras = new JogoDePalavras();

            lblNovoJogo.Visible = true;

            pnlTeclas.Enabled = true;

            ConfigurarNovoJogo();

            ComecarPartida();
        }

        private void DarPalpite(object? sender, EventArgs e)
        {
            Button botaoClicado = (Button)sender;
            ColocarLetra(Convert.ToChar(botaoClicado.Text[0]));
        }

        private void ObterPalavra()
        {
            jogoPalavras.palavraCorreta = "";

            foreach (Control txtLetra in pnlMostrarLetra.Controls)
            {
                if (pnlMostrarLetra.GetRow(txtLetra) == jogoPalavras.contador)
                {
                    jogoPalavras.palavraCorreta += txtLetra.Text;
                }
            }
        }

        private void ColocarLetra(char letraTeclado)
        {
            foreach (Control txtLetra in pnlMostrarLetra.Controls)
            {
                if (txtLetra is TextBox && pnlMostrarLetra.GetRow(txtLetra) == jogoPalavras.contador && txtLetra.Text == "")
                {
                    txtLetra.Text = letraTeclado.ToString();
                    break;
                }
            }
        }

        private void ConfirmarPalavraEscrita()
        {
            ObterPalavra();

            if (jogoPalavras.VerificaSePalavraEstaCompleta())

                FinalizarPartida();
        }


        //metodos de verificacao para as letras escolhidas
        private void VerificarLetrasVermelhas()
        {
            foreach (Control txtLetra in pnlMostrarLetra.Controls)
            {
                if (pnlMostrarLetra.GetRow(txtLetra) != jogoPalavras.contador)
                    continue;

                txtLetra.BackColor = Color.Firebrick;

                foreach (Control btnTeclado in pnlTeclas.Controls)
                {
                    if (btnTeclado.Text == "Enter")
                        continue;

                    if (jogoPalavras.CompararLetrasIguais(Convert.ToChar(btnTeclado.Text[0]), Convert.ToChar(txtLetra.Text[0])))
                        btnTeclado.BackColor = Color.Firebrick;
                }
            }
        }

        private void VerificarLetrasAmarelas()
        {
            foreach (char letraPalavraSecreta in jogoPalavras.palavraSecreta)
            {
                foreach (Control txtLetra in pnlMostrarLetra.Controls)
                {
                    if (pnlMostrarLetra.GetRow(txtLetra) != jogoPalavras.contador)
                        continue;

                    if (jogoPalavras.CompararLetrasIguais(Convert.ToChar(txtLetra.Text), letraPalavraSecreta))
                    {
                        txtLetra.BackColor = Color.LightSalmon;

                        foreach (Control btnTeclado in pnlTeclas.Controls)
                        {
                            if (btnTeclado.Text == txtLetra.Text && btnTeclado.BackColor != Color.YellowGreen)
                            {
                                btnTeclado.BackColor = Color.LightSalmon;
                            }
                        }
                    }
                }
            }
        }

        private void VerificarLetrasVerdes()
        {
            List<Control> txtListaLetras = new List<Control>();

            foreach (Control txtBox in pnlMostrarLetra.Controls)
            {
                if (pnlMostrarLetra.GetRow(txtBox) == jogoPalavras.contador)
                    txtListaLetras.Add(txtBox);
            }

            for (int letra = 0; letra < jogoPalavras.palavraSecreta.Length && letra < txtListaLetras.Count; letra++)
            {
                char letraNoTxt = Convert.ToChar(txtListaLetras[letra].Text);
                char letraSecreta = jogoPalavras.palavraSecreta[letra];

                if (jogoPalavras.CompararLetrasIguais(letraNoTxt, letraSecreta))
                {
                    txtListaLetras[letra].BackColor = Color.YellowGreen;
                    txtListaLetras[letra].Text = letraSecreta.ToString();

                    foreach (Control btnTeclado in pnlTeclas.Controls)
                    {
                        if (btnTeclado.Text == txtListaLetras[letra].Text)
                            btnTeclado.BackColor = Color.YellowGreen;
                    }
                }
            }
        }


        //partida
        private void ComecarPartida()
        {
            foreach (Control txtLetra in pnlMostrarLetra.Controls)
            {
                if (pnlMostrarLetra.GetRow(txtLetra) == jogoPalavras.contador)
                {
                    txtLetra.BackColor = Color.FromArgb(255, 128, 128);
                }
            }
        }

        private void FinalizarPartida()
        {
            VerificarLetrasVermelhas();

            VerificarLetrasAmarelas();

            VerificarLetrasVerdes();

            if (jogoPalavras.ComparaSeGanhou())
                GanharJogo();

            else if (jogoPalavras.ComparaSePerdeu())
                PerderJogo();

            jogoPalavras.contador++;

            ComecarPartida();
        }


        //ganhou ou perdeu
        private void GanharJogo()
        {
            MessageBox.Show("Parabéns, você venceu!");

            lblNovoJogo.Visible = true;
            pnlTeclas.Enabled = false;
        }

        private void PerderJogo()
        {
            MessageBox.Show("que pena, você perdeu, tente novamente...!");

            lblNovoJogo.Visible = true;
            pnlTeclas.Enabled = false;
        }


        //novo jogo
        private void ConfigurarNovoJogo()
        {
            foreach (Control txtTabelaLetra in pnlMostrarLetra.Controls)
            {
                txtTabelaLetra.Text = "";
                txtTabelaLetra.BackColor = Color.FromArgb(255, 128, 128);
            }

            foreach (Control btnTeclado in pnlTeclas.Controls)
            {
                btnTeclado.BackColor = Color.Tomato;
            }
        }

        private void NovoJogo(object sender, EventArgs e)
        {
            ComecarJogo();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            ConfirmarPalavraEscrita();
        }
    }
}
