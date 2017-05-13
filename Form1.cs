using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace JezioraSQL
{
    public partial class Form1 : Form
    {
        DataTable Tabela; SqlCommand Sta;
        int Pozycja;
        void Wstaw_Dane()
        {
            txt_ident.Text = Tabela.Rows[Pozycja]["ident"].ToString();
            txt_nazwa.Text = Tabela.Rows[Pozycja]["nazwa_jeziora"].ToString();
            txt_powierzchnia.Text = Tabela.Rows[Pozycja]["powierzchnia_km"].ToString();
            txt_glebokosc.Text = Tabela.Rows[Pozycja]["glebokosc_m"].ToString();
            txt_miejscowosc.Text = Tabela.Rows[Pozycja]["miejscowosc"].ToString();
            if (int.Parse(Tabela.Rows[Pozycja]["strefa_ciszy"].ToString()) == 1)
                chb_strefaciszy.Checked = true;
            else
                chb_strefaciszy.Checked = false;
            if (int.Parse(Tabela.Rows[Pozycja]["kapielisko"].ToString()) == 1)
                chb_kapielisko.Checked = true;
            else
                chb_kapielisko.Checked = false;

            Msg.Text = "OK";
        }
        public Form1()
        {
            InitializeComponent();
            txt_ident.Enabled = false;
            btn_add.Visible = false;
            btn_cancel.Visible = false;
            SqlConnection MyConn = new SqlConnection(global::JezioraSQL.Properties.Settings.Default.JezioraConnectionString);
            try
            {
                MyConn.Open();
            }
            catch (Exception ex) { Msg.Text = "Brak połączenia z Bazą danych"; return; }
            Sta = MyConn.CreateCommand();
            Sta.CommandText = "select * from Jeziora";
            SqlDataReader Reader = Sta.ExecuteReader();
            Tabela = new DataTable(); Tabela.Load(Reader);
            Pozycja = 0;
            if (Tabela.Rows.Count > 0) Wstaw_Dane();
            else Msg.Text = "brak wierszy w tabeli";
        }

        private void btn_first_Click(object sender, EventArgs e)
        {
            if (Tabela.Rows.Count > 0)
            {
                Pozycja = 0; Wstaw_Dane();
            }
            else Msg.Text = "brak wierszy w tabeli";
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (Pozycja > 0)
            {
                Pozycja--; Wstaw_Dane();
            }
            else Msg.Text = "początek tabeli";
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (Pozycja < Tabela.Rows.Count - 1)
            {
                Pozycja++; Wstaw_Dane();
            }
            else Msg.Text = "koniec tabeli";
        }

        private void btn_last_Click(object sender, EventArgs e)
        {
            if (Tabela.Rows.Count > 0)
            {
                Pozycja = Tabela.Rows.Count - 1; Wstaw_Dane();
            }
            else Msg.Text = "brak wierszy w tabeli";
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            
            Sta.CommandText = "update Jeziora set nazwa_jeziora='" + txt_nazwa.Text + "' where ident=" + txt_ident.Text;
            Sta.CommandText = "update Jeziora set powierzchnia_km=" + txt_powierzchnia.Text + " where ident=" + txt_ident.Text;
            Sta.CommandText = "update Jeziora set glebokosc_m=" + txt_glebokosc.Text + " where ident=" + txt_ident.Text;
            Sta.CommandText = "update Jeziora set miejscowosc='" + txt_miejscowosc.Text + "' where ident=" + txt_ident.Text;
            if (chb_strefaciszy.Checked == true)
                Sta.CommandText = "update Jeziora set strefa_ciszy=" + 1 + " where ident=" + txt_ident.Text;
            else
                Sta.CommandText = "update Jeziora set strefa_ciszy=" + 0 + " where ident=" + txt_ident.Text;
            if (chb_kapielisko.Checked == true)
                Sta.CommandText = "update Jeziora set kapielisko=" + 1 + " where ident=" + txt_ident.Text;
            else
                Sta.CommandText = "update Jeziora set kapielisko=" + 0 + " where ident=" + txt_ident.Text;

            Tabela.Rows[Pozycja]["nazwa_jeziora"] = txt_nazwa.Text;
            Tabela.Rows[Pozycja]["powierzchnia_km"] = txt_powierzchnia.Text;
            Tabela.Rows[Pozycja]["glebokosc_m"] = txt_glebokosc.Text;
            Tabela.Rows[Pozycja]["miejscowosc"] = txt_miejscowosc.Text;
            if (chb_strefaciszy.Checked == true)
            {
                chb_strefaciszy.Checked = true;
                Tabela.Rows[Pozycja]["strefa_ciszy"] = 1;
            }

            else
            {
                chb_strefaciszy.Checked = false;
                Tabela.Rows[Pozycja]["strefa_ciszy"] = 0;
            }

            if (chb_kapielisko.Checked == true)
            {
                chb_kapielisko.Checked = true;
                Tabela.Rows[Pozycja]["kapielisko"] = 1;
            }

            else
            {
                chb_kapielisko.Checked = false;
                Tabela.Rows[Pozycja]["kapielisko"] = 0;
            }


            Msg.Text = "Updated";

        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            Sta.CommandText = "delete from Jeziora where ident=" + txt_ident.Text;
            Sta.ExecuteNonQuery();
            Tabela.Rows.RemoveAt(Pozycja);
            if (Pozycja == Tabela.Rows.Count)
            {
                Pozycja--;
            }
            Wstaw_Dane();
            Msg.Text = "Deleted";
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            btn_add.Visible = true;
            btn_cancel.Visible = true;
            btn_delete.Visible = false;
            btn_last.Visible = false;
            btn_new.Visible = false;
            btn_next.Visible = false;
            btn_first.Visible = false;
            btn_prev.Visible = false;
            btn_update.Visible = false;
            txt_ident.Enabled = true;
            txt_ident.Text = txt_nazwa.Text = txt_powierzchnia.Text = txt_glebokosc.Text = txt_miejscowosc.Text = "";
            chb_strefaciszy.Checked = false;
            chb_kapielisko.Checked = false;
            Msg.Text = "Wprowadź dane i kliknij Dodaj";
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            btn_add.Visible = false;
            btn_cancel.Visible = false;
            btn_delete.Visible = true;
            btn_last.Visible = true;
            btn_new.Visible = true;
            btn_next.Visible = true;
            btn_first.Visible = true;
            btn_prev.Visible = true;
            btn_update.Visible = true;
            txt_ident.Enabled = false;
            Sta.CommandText = "insert into Jeziora(ident,nazwa_jeziora,powierzchnia_km,glebokosc_m,miejscowosc,strefa_ciszy,kapielisko) values(@ident,@nazwa_jeziora,@powierzchnia_km,@glebokosc_m,@miejscowosc,@strefa_ciszy,@kapielisko)";
            Sta.Parameters.AddWithValue("@ident", int.Parse(txt_ident.Text));
            Sta.Parameters.AddWithValue("@nazwa_jeziora", txt_nazwa.Text);
            Sta.Parameters.AddWithValue("@powierzchnia_km", int.Parse(txt_powierzchnia.Text));
            Sta.Parameters.AddWithValue("@glebokosc_m", int.Parse(txt_glebokosc.Text));
            Sta.Parameters.AddWithValue("@miejscowosc", txt_miejscowosc.Text);
            if (chb_strefaciszy.Checked == true)
            {
                Sta.Parameters.AddWithValue("@strefa_ciszy", 1);
            }
            else
                Sta.Parameters.AddWithValue("@strefa_ciszy", 0);
            if (chb_kapielisko.Checked == true)
            {
                Sta.Parameters.AddWithValue("@kapielisko", 1);
            }
            else
                Sta.Parameters.AddWithValue("@kapielisko", 0);
            
            Sta.ExecuteNonQuery();
            DataRow Wiersz = Tabela.NewRow(); // krwadratowe nawiasy to index property 
            Wiersz[0] = txt_ident.Text; Wiersz[1] = txt_nazwa.Text; Wiersz[2] = txt_powierzchnia.Text; Wiersz[3] = txt_glebokosc.Text;
            Wiersz[4] = txt_miejscowosc.Text;
            if (chb_strefaciszy.Checked == true)
            {
                Wiersz[5] = 1;
            }
            else
                Wiersz[5] = 0;
            if (chb_kapielisko.Checked == true)
            {
                Wiersz[6] = 1;
            }
            else
                Wiersz[6] = 0;
            Tabela.Rows.Add(Wiersz);
            Pozycja = Tabela.Rows.Count - 1;
            Msg.Text = "Added";
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
                
        }
    }
}
