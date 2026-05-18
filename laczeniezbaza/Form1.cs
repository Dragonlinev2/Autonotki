using System;
using System.Windows.Forms;
using Npgsql;

namespace laczeniezbaza
{
    public partial class Form1 : Form
    {
        string polaczenie =
            "Host=localhost;Port=5432;Username=postgres;Password=test123;Database=AUTONOTKI;";

        public Form1()
        {
            InitializeComponent();

            texthaslo.PasswordChar = '*';
            komunikat.Text = "";
        }

        private void guzik_Click(object sender, EventArgs e)
        {
            string login = textlogin.Text.Trim();
            string haslo = texthaslo.Text.Trim();

            if (login == "" || haslo == "")
            {
                komunikat.Text = "Wpisz login i haslo";
                return;
            }

            try
            {
                string rola = sprawdz_logowanie(login, haslo);

                if (rola == null)
                {
                    komunikat.Text = "Bledny login lub haslo";
                }
                else
                {
                    komunikat.Text = "Zalogowano jako: " + rola.ToLower();

                    if (rola == "ADMIN")
                    {
                        MessageBox.Show("przekierowanie do strony administratora");
                    }
                    else if (rola == "PRACOWNIK")
                    {
                        MessageBox.Show("przekierowanie do strony pracownika");
                    }
                }
            }
            catch (Exception blad)
            {
                MessageBox.Show("blad polaczenia z baza:\n" + blad.Message);
            }
        }

        private string sprawdz_logowanie(string login, string haslo)
        {
            using (var conn = new NpgsqlConnection(polaczenie))
            {
                conn.Open();

                string sql = @"
                    SELECT rola
                    FROM UZYTKOWNICY
                    WHERE login = @login AND haslo = @haslo";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@haslo", haslo);

                    object wynik = cmd.ExecuteScalar();

                    if (wynik != null)
                    {
                        return wynik.ToString();
                    }
                }
            }

            return null;
        }
    }
}