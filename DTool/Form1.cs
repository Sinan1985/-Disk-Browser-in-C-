using Microsoft.VisualBasic;

namespace DTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //ShowDrives(); // Load drives when form starts
        }

        private void ShowDrives()
        {
            treeView1.BeginUpdate();
            string[] drives = Directory.GetLogicalDrives();
            foreach (string adrive in drives)
            {
                TreeNode tn = new TreeNode(adrive);
                treeView1.Nodes.Add(tn);
                AddDirs(tn);
            }
            treeView1.EndUpdate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowDrives();
        }

        public void ShowFileNames()
        {
            DirectoryInfo di = new DirectoryInfo(treeView1.SelectedNode.FullPath);
            FileInfo[] fiarray = { };
            ListViewItem item;

            listView1.Items.Clear();

            if (di.Exists)
            {
                fiarray = di.GetFiles();
            }

            listView1.BeginUpdate();
            foreach (FileInfo fi in fiarray)
            {
                item = new ListViewItem(fi.Name);
                listView1.Items.Add(item);
            }
            listView1.EndUpdate();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowFileNames();
        }

        public void AddDirs(TreeNode tn)
        {
            string path = tn.FullPath;
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] diarray = { };

            try
            {
                if (di.Exists)
                    diarray = di.GetDirectories();
            }
            catch
            {
                return;
            }

            foreach (DirectoryInfo d in diarray)
            {
                TreeNode tndir = new TreeNode(d.Name);
                tn.Nodes.Add(tndir);
            }

        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            treeView1.BeginUpdate();
            foreach (TreeNode tn in e.Node.Nodes)
            {
                AddDirs(tn);
            }
            treeView1.EndUpdate();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            ShowDrives();
        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            ShowFileNames();
        }

        private void treeView1_BeforeExpand_1(object sender, TreeViewCancelEventArgs e)
        {
            treeView1.BeginUpdate();
            foreach (TreeNode tn in e.Node.Nodes) {
                AddDirs(tn);
            }
            treeView1 .EndUpdate();
        }
    }
}
