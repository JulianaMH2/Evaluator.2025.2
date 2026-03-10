using Evaluator.Core;

namespace Evaluator.UI.Windows
{
    public partial class Form1 : Form
    {
        private string _expression = string.Empty;

        public Form1()
        {
            InitializeComponent();
            BuildCalculator();
        }

        private void BuildCalculator()
        {
            this.Text = "Calculadora";
            this.Size = new Size(320, 480);
            this.BackColor = Color.FromArgb(18, 10, 30);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Pantalla
            var display = new TextBox
            {
                Name = "display",
                Text = "0",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Right,
                BackColor = Color.FromArgb(30, 15, 50),
                ForeColor = Color.FromArgb(220, 180, 255),
                ReadOnly = true,
                Size = new Size(284, 60),
                Location = new Point(10, 10),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(display);

            var buttons = new[]
            {
                ("C",  0, 0), ("(",  0, 1), (")",  0, 2), ("/",  0, 3),
                ("7",  1, 0), ("8",  1, 1), ("9",  1, 2), ("*",  1, 3),
                ("4",  2, 0), ("5",  2, 1), ("6",  2, 2), ("-",  2, 3),
                ("1",  3, 0), ("2",  3, 1), ("3",  3, 2), ("+",  3, 3),
                (".",  4, 0), ("0",  4, 1), ("⌫",  4, 2), ("=",  4, 3),
            };

            foreach (var (text, row, col) in buttons)
            {
                var btn = new Button
                {
                    Text = text,
                    Size = new Size(65, 65),
                    Location = new Point(10 + col * 70, 85 + row * 70),
                    Font = new Font("Segoe UI", 16, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };

                if (text == "=")
                {
                    btn.BackColor = Color.FromArgb(120, 40, 200);
                    btn.ForeColor = Color.White;
                }
                else if (text is "+" or "-" or "*" or "/")
                {
                    btn.BackColor = Color.FromArgb(70, 30, 100);
                    btn.ForeColor = Color.FromArgb(220, 160, 255);
                }
                else if (text is "C" or "⌫")
                {
                    btn.BackColor = Color.FromArgb(80, 20, 80);
                    btn.ForeColor = Color.FromArgb(255, 100, 255);
                }
                else if (text is "(" or ")")
                {
                    btn.BackColor = Color.FromArgb(50, 20, 80);
                    btn.ForeColor = Color.FromArgb(200, 150, 255);
                }
                else
                {
                    btn.BackColor = Color.FromArgb(45, 20, 70);
                    btn.ForeColor = Color.FromArgb(230, 210, 255);
                }

                btn.FlatAppearance.BorderSize = 0;
                btn.Tag = text;
                btn.Click += Button_Click;
                this.Controls.Add(btn);
            }
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            var display = (TextBox)this.Controls["display"]!;
            var btn = (Button)sender!;
            var value = btn.Tag!.ToString()!;

            switch (value)
            {
                case "C":
                    _expression = string.Empty;
                    display.Text = "0";
                    break;

                case "⌫":
                    if (_expression.Length > 0)
                        _expression = _expression[..^1];
                    display.Text = _expression == string.Empty ? "0" : _expression;
                    break;

                case "=":
                    try
                    {
                        var result = ExpressionEvaluator.Evaluate(_expression);
                        display.Text = result.ToString();
                        _expression = result.ToString();
                    }
                    catch
                    {
                        display.Text = "Error";
                        _expression = string.Empty;
                    }
                    break;

                default:
                    _expression += value;
                    display.Text = _expression;
                    break;
            }
        }
    }
}
    