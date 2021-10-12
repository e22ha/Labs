
namespace CustomGallery
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.roundGallery1 = new CustomGallery.RoundGallery();
            this.recGallery1 = new CustomGallery.RecGallery();
            this.SuspendLayout();
            // 
            // roundGallery1
            // 
            this.roundGallery1.Font = new System.Drawing.Font("Microsoft YaHei UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roundGallery1.ForeColor = System.Drawing.Color.White;
            this.roundGallery1.Location = new System.Drawing.Point(463, 126);
            this.roundGallery1.Name = "roundGallery1";
            this.roundGallery1.Size = new System.Drawing.Size(194, 182);
            this.roundGallery1.TabIndex = 1;
            this.roundGallery1.UseVisualStyleBackColor = true;

            // 
            // recGallery1
            // 
            this.recGallery1.Font = new System.Drawing.Font("Microsoft YaHei UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recGallery1.ForeColor = System.Drawing.Color.White;
            this.recGallery1.Location = new System.Drawing.Point(60, 104);
            this.recGallery1.Name = "recGallery1";
            this.recGallery1.Size = new System.Drawing.Size(240, 216);
            this.recGallery1.TabIndex = 0;
            this.recGallery1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.roundGallery1);
            this.Controls.Add(this.recGallery1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private RecGallery recGallery1;
        private RoundGallery roundGallery1;
    }
}

