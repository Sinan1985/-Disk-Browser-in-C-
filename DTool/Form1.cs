using Microsoft.VisualBasic;
using System.Diagnostics;

namespace DTool
{
    public partial class Form1 : Form
    {
        ImageList imageList1;
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
            Icon dockerIcon = new Icon("docker.ico");//No extension files will hv Docker Icon
            imageList1 = new ImageList();
            listView1.Items.Clear();
            listView1.SmallImageList = imageList1;

            if (di.Exists)
            {
                fiarray = di.GetFiles();
            }

            listView1.BeginUpdate();
            foreach (FileInfo fi in fiarray)
            {
                Icon iconForFile;
                item = new ListViewItem(fi.Name);
                listView1.Items.Add(item);
                iconForFile = SystemIcons.WinLogo;

                //If the ImageList does NOT already contain an icon for this file extension,
                //then extract an icon from the file and add it to the ImageList
                if (!imageList1.Images.ContainsKey(fi.Extension))
                {
                    iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(fi.FullName);
                    imageList1.Images.Add(fi.Extension, iconForFile);
                }
                item.ImageKey = fi.Extension;
                item.SubItems.Add(fi.Length.ToString() + "bytes");
                item.SubItems.Add(fi.LastWriteTime.ToString());
                item.SubItems.Add(GetAtts(fi));

                string key = string.IsNullOrEmpty(fi.Extension) ? "noext" : fi.Extension;
                item.ImageKey = key;
                if (!imageList1.Images.ContainsKey(key))
                {
                    
                    try
                    {
                        iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(fi.FullName);
                    }
                    catch
                    {
                        iconForFile = SystemIcons.WinLogo; // fallback if extraction fails
                    }
                    //imageList1.Images.Add(key, iconForFile);
                    imageList1.Images.Add("noext", dockerIcon);
                }

                



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

        private string GetAtts(FileInfo fi)
        {
            string atts = "";
            if ((fi.Attributes & FileAttributes.Archive) != 0)
                atts += "A";
            if ((fi.Attributes & FileAttributes.Hidden) != 0)
                atts += "H";
            if ((fi.Attributes & FileAttributes.ReadOnly) != 0)
                atts += "R";
            if ((fi.Attributes & FileAttributes.System) != 0)
                atts += "S";
            return atts;
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
            foreach (TreeNode tn in e.Node.Nodes)
            {
                AddDirs(tn);
            }
            treeView1.EndUpdate();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            string diskfile = treeView1.SelectedNode.FullPath;
            if (!diskfile.EndsWith("\\"))
                diskfile += "\\";
            diskfile += listView1.FocusedItem.Text;
            if (File.Exists(diskfile))
                Process.Start(new ProcessStartInfo { FileName = diskfile, UseShellExecute = true });
        }
    }
}
