namespace ArmaSheduler
{
    partial class ArmaSheduler
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.logger = new System.Diagnostics.EventLog();
            this.bigBen = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.logger)).BeginInit();
            // 
            // ArmaSheduler
            // 
            this.ServiceName = "ArmaSheduler";
            ((System.ComponentModel.ISupportInitialize)(this.logger)).EndInit();

        }

        #endregion

        private System.Diagnostics.EventLog logger;
        private System.Windows.Forms.Timer bigBen;
    }
}
