namespace laczeniezbaza
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.guzik = new System.Windows.Forms.Button();
            this.komunikat = new System.Windows.Forms.Label();
            this.texthaslo = new System.Windows.Forms.TextBox();
            this.textlogin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // guzik
            // 
            this.guzik.Location = new System.Drawing.Point(12, 99);
            this.guzik.Name = "guzik";
            this.guzik.Size = new System.Drawing.Size(75, 23);
            this.guzik.TabIndex = 0;
            this.guzik.Text = "Zaloguj sie";
            this.guzik.UseVisualStyleBackColor = true;
            this.guzik.Click += new System.EventHandler(this.guzik_Click);
            // 
            // komunikat
            // 
            this.komunikat.AutoSize = true;
            this.komunikat.Location = new System.Drawing.Point(18, 125);
            this.komunikat.Name = "komunikat";
            this.komunikat.Size = new System.Drawing.Size(56, 13);
            this.komunikat.TabIndex = 1;
            this.komunikat.Text = "komunikat";
            // 
            // texthaslo
            // 
            this.texthaslo.Location = new System.Drawing.Point(12, 73);
            this.texthaslo.Name = "texthaslo";
            this.texthaslo.Size = new System.Drawing.Size(100, 20);
            this.texthaslo.TabIndex = 2;
            // 
            // textlogin
            // 
            this.textlogin.Location = new System.Drawing.Point(15, 25);
            this.textlogin.Name = "textlogin";
            this.textlogin.Size = new System.Drawing.Size(100, 20);
            this.textlogin.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Podaj login";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Podaj haslo";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textlogin);
            this.Controls.Add(this.texthaslo);
            this.Controls.Add(this.komunikat);
            this.Controls.Add(this.guzik);
            this.Name = "Form1";
            this.Text = "Logowanie";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button guzik;
        private System.Windows.Forms.Label komunikat;
        private System.Windows.Forms.TextBox texthaslo;
        private System.Windows.Forms.TextBox textlogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

