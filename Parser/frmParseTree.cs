using System;
using System.Drawing;
using System.Windows.Forms;

namespace Parser
{
    public partial class frmParseTree : Form
    {
        private const int NODE_WIDTH = 100;
        private const int NODE_HEIGHT = 20;
        private const int NODE_SPACE = 10;
        private bool looping = true;
        
        public Parser Parser = new Parser();

        public frmParseTree()
        {
            InitializeComponent();
        }

        private void frmParseTree_Load(object sender, EventArgs e)
        {

        }

        private void frmParseTree_Shown(object sender, EventArgs e)
        {
            try
            {
                while (looping)
                {
                    canvas.Image = DrawTree();
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public Bitmap DrawTree()
        {
            Bitmap bitmap = new Bitmap(canvas.Width, canvas.Height);
            Graphics g = Graphics.FromImage(bitmap);

            g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
            if (Parser.Branch != null)
            {
                DrawBranch(g, Parser.Branch, 0, 0);
            }

            return bitmap;
        }

        public void DrawBranch(Graphics g, Branch branch, int depth, int count)
        {
            if (branch.Children != null)
            {
                for (int i = 0; i < branch.Children.Length; i++)
                {
                    if (branch.Children[i] != null)
                        DrawBranch(g, branch.Children[i], depth + 1, i);
                }
            }
            
            g.FillRectangle(Brushes.DarkGray, count * (NODE_WIDTH + NODE_SPACE), depth * (NODE_HEIGHT + NODE_SPACE), NODE_WIDTH, NODE_HEIGHT);
            g.DrawRectangle(Pens.Green, count * (NODE_WIDTH + NODE_SPACE), depth * (NODE_HEIGHT + NODE_SPACE), NODE_WIDTH, NODE_HEIGHT);
            g.DrawString(branch.Title, new Font(FontFamily.GenericSansSerif, 8.0f), Brushes.Black, count * (NODE_WIDTH + NODE_SPACE), depth * (NODE_HEIGHT + NODE_SPACE));
        }

        private void frmParseTree_FormClosing(object sender, FormClosingEventArgs e)
        {
            looping = false;
        }
    }
}
