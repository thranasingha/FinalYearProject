using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SourceGrid2.Cells.Real;
using WeifenLuo.WinFormsUI;

using AForge;
using AForge.Math;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Imaging.Textures;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IPLab
{
    /// <summary>
    /// Summary description for ImageDoc.
    /// </summary>
    public class ImageDoc : Content
    {
        private System.Drawing.Bitmap backup = null;
        private System.Drawing.Bitmap image = null;
        private string fileName = null;
        private int width;
        private double lineLength = 0;
        private int height;
        private float zoom = 1;
        private IDocumentsHost host = null;


        private bool cropping = false;
        private bool dragging = false;
        private bool drawingLine = false;
        private bool temp = false;

        private Point start, end, startW, endW;
        private Point imageStart, imageEnd;
        private Point LineStart;
        private List<string> tempMetadataList = new List<string>(); 

        #region form items

        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem imageItem;
        private System.Windows.Forms.MenuItem filtersItem;
        private System.Windows.Forms.MenuItem grayscaleColorFiltersItem;
        private System.Windows.Forms.MenuItem backImageItem;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem z10ImageItem;
        private System.Windows.Forms.MenuItem z25ImageItem;
        private System.Windows.Forms.MenuItem z50ImageItem;
        private System.Windows.Forms.MenuItem z75ImageItem;
        private System.Windows.Forms.MenuItem z100ImageItem;
        private System.Windows.Forms.MenuItem z150ImageItem;
        private System.Windows.Forms.MenuItem z200ImageItem;
        private System.Windows.Forms.MenuItem z400ImageItem;
        private System.Windows.Forms.MenuItem z500ImageItem;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem zoomInImageItem;
        private System.Windows.Forms.MenuItem zoomOutImageItem;
        private System.Windows.Forms.MenuItem menuItem11;
        private System.Windows.Forms.MenuItem zoomFitImageItem;
        private System.Windows.Forms.MenuItem thresholdBinaryFiltersItem;
        private System.Windows.Forms.MenuItem flipImageItem;
        private System.Windows.Forms.MenuItem mirrorItem;
        private System.Windows.Forms.MenuItem rotateImageItem;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.MenuItem cropImageItem;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem resizeFiltersItem;
        private System.Windows.Forms.MenuItem rotateFiltersItem;
        private System.Windows.Forms.MenuItem edgeFiltersItem;
        private System.Windows.Forms.MenuItem homogenityEdgeFiltersItem;
        private System.Windows.Forms.MenuItem differenceEdgeFiltersItem;
        private System.Windows.Forms.MenuItem sobelEdgeFiltersItem;
        private System.Windows.Forms.MenuItem cannyEdgeFiltersItem;
        private MenuItem FilterItem;
        private MenuItem thresholdingSegment;

        private System.ComponentModel.IContainer components;

        #endregion
        private MenuItem lineVarienceMenu;
        private MenuItem drowLineMenu;
        private MenuItem analyseMenu;
        private MenuItem resxultsMenu;
        private MenuItem csvResult;

        public string checkerBoardValue;
        private MenuItem verificationMenu;
        private MenuItem checkerboardMenu;
        private MenuItem ditheringMenu;
        private MenuItem edgeVerienceMenu;
        //public string dytheringValue;
        //public string edgeaVerienceValue;

        //Dithering Variables
        public System.Drawing.Bitmap ditheringImage = null;
        private int cropCount = 0;
        int[] colorArray1;
        int[] colorArray2;
        int[] colorArray3;
        private PictureBox pictureBox1;

        // Image property
        public Bitmap Image
        {
            get { return image; }
        }
        // Width property
        public int ImageWidth
        {
            get { return width; }
        }
        // Height property
        public int ImageHeight
        {
            get { return height; }
        }
        // Zoom property
        public float Zoom
        {
            get { return zoom; }
        }
        // FileName property
        // return file name if the document was created from file or null
        public string FileName
        {
            get { return fileName; }
        }


        // Events
        public delegate void SelectionEventHandler(object sender, SelectionEventArgs e);

        public event EventHandler DocumentChanged;
        public event EventHandler ZoomChanged;
        public event SelectionEventHandler MouseImagePosition;
        public event SelectionEventHandler SelectionChanged;


        //Background workers
        BackgroundWorker bw;


        // Constructors
        private ImageDoc(IDocumentsHost host)
        {
            this.host = host;
        }
        // Construct from file
        public ImageDoc(string fileName, IDocumentsHost host)
            : this(host)
        {
            try
            {
                // load image
                image = (Bitmap)Bitmap.FromFile(fileName);

                // format image
                AForge.Imaging.Image.FormatImage(ref image);

                this.fileName = fileName;
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
            }

            Init();
        }
        // Construct from image
        public ImageDoc(Bitmap image, IDocumentsHost host)
            : this(host)
        {
            this.image = image;
            AForge.Imaging.Image.FormatImage(ref this.image);

            Init();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (image != null)
                {
                    image.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.imageItem = new System.Windows.Forms.MenuItem();
            this.backImageItem = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.z10ImageItem = new System.Windows.Forms.MenuItem();
            this.z25ImageItem = new System.Windows.Forms.MenuItem();
            this.z50ImageItem = new System.Windows.Forms.MenuItem();
            this.z75ImageItem = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.z100ImageItem = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.z150ImageItem = new System.Windows.Forms.MenuItem();
            this.z200ImageItem = new System.Windows.Forms.MenuItem();
            this.z400ImageItem = new System.Windows.Forms.MenuItem();
            this.z500ImageItem = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.zoomInImageItem = new System.Windows.Forms.MenuItem();
            this.zoomOutImageItem = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.zoomFitImageItem = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.flipImageItem = new System.Windows.Forms.MenuItem();
            this.mirrorItem = new System.Windows.Forms.MenuItem();
            this.rotateImageItem = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.cropImageItem = new System.Windows.Forms.MenuItem();
            this.filtersItem = new System.Windows.Forms.MenuItem();
            this.grayscaleColorFiltersItem = new System.Windows.Forms.MenuItem();
            this.thresholdBinaryFiltersItem = new System.Windows.Forms.MenuItem();
            this.edgeFiltersItem = new System.Windows.Forms.MenuItem();
            this.homogenityEdgeFiltersItem = new System.Windows.Forms.MenuItem();
            this.differenceEdgeFiltersItem = new System.Windows.Forms.MenuItem();
            this.sobelEdgeFiltersItem = new System.Windows.Forms.MenuItem();
            this.cannyEdgeFiltersItem = new System.Windows.Forms.MenuItem();
            this.resizeFiltersItem = new System.Windows.Forms.MenuItem();
            this.rotateFiltersItem = new System.Windows.Forms.MenuItem();
            this.FilterItem = new System.Windows.Forms.MenuItem();
            this.thresholdingSegment = new System.Windows.Forms.MenuItem();
            this.resxultsMenu = new System.Windows.Forms.MenuItem();
            this.csvResult = new System.Windows.Forms.MenuItem();
            this.lineVarienceMenu = new System.Windows.Forms.MenuItem();
            this.drowLineMenu = new System.Windows.Forms.MenuItem();
            this.analyseMenu = new System.Windows.Forms.MenuItem();
            this.verificationMenu = new System.Windows.Forms.MenuItem();
            this.checkerboardMenu = new System.Windows.Forms.MenuItem();
            this.ditheringMenu = new System.Windows.Forms.MenuItem();
            this.edgeVerienceMenu = new System.Windows.Forms.MenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.imageItem,
            this.filtersItem,
            this.FilterItem,
            this.resxultsMenu,
            this.lineVarienceMenu,
            this.verificationMenu});
            // 
            // imageItem
            // 
            this.imageItem.Index = 0;
            this.imageItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.backImageItem,
            this.menuItem5,
            this.menuItem10,
            this.flipImageItem,
            this.mirrorItem,
            this.rotateImageItem,
            this.menuItem3,
            this.cropImageItem});
            this.imageItem.MergeOrder = 1;
            this.imageItem.Text = "&Image";
            this.imageItem.Popup += new System.EventHandler(this.imageItem_Popup);
            // 
            // backImageItem
            // 
            this.backImageItem.Index = 0;
            this.backImageItem.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.backImageItem.Text = "&Back";
            this.backImageItem.Click += new System.EventHandler(this.backImageItem_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.z10ImageItem,
            this.z25ImageItem,
            this.z50ImageItem,
            this.z75ImageItem,
            this.menuItem7,
            this.z100ImageItem,
            this.menuItem6,
            this.z150ImageItem,
            this.z200ImageItem,
            this.z400ImageItem,
            this.z500ImageItem,
            this.menuItem8,
            this.zoomInImageItem,
            this.zoomOutImageItem,
            this.menuItem11,
            this.zoomFitImageItem});
            this.menuItem5.Text = "&Zoom";
            // 
            // z10ImageItem
            // 
            this.z10ImageItem.Index = 0;
            this.z10ImageItem.Text = "10%";
            this.z10ImageItem.Click += new System.EventHandler(this.zoomItem_Click);
            // 
            // z25ImageItem
            // 
            this.z25ImageItem.Index = 1;
            this.z25ImageItem.Text = "25%";
            this.z25ImageItem.Click += new System.EventHandler(this.zoomItem_Click);
            // 
            // z50ImageItem
            // 
            this.z50ImageItem.Index = 2;
            this.z50ImageItem.Text = "50%";
            this.z50ImageItem.Click += new System.EventHandler(this.zoomItem_Click);
            // 
            // z75ImageItem
            // 
            this.z75ImageItem.Index = 3;
            this.z75ImageItem.Text = "75%";
            this.z75ImageItem.Click += new System.EventHandler(this.zoomItem_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 4;
            this.menuItem7.Text = "-";
            // 
            // z100ImageItem
            // 
            this.z100ImageItem.Index = 5;
            this.z100ImageItem.Shortcut = System.Windows.Forms.Shortcut.Ctrl0;
            this.z100ImageItem.Text = "100%";
            this.z100ImageItem.Click += new System.EventHandler(this.zoomItem_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 6;
            this.menuItem6.Text = "-";
            // 
            // z150ImageItem
            // 
            this.z150ImageItem.Index = 7;
            this.z150ImageItem.Text = "150%";
            this.z150ImageItem.Click += new System.EventHandler(this.zoomItem_Click);
            // 
            // z200ImageItem
            // 
            this.z200ImageItem.Index = 8;
            this.z200ImageItem.Text = "200%";
            this.z200ImageItem.Click += new System.EventHandler(this.zoomItem_Click);
            // 
            // z400ImageItem
            // 
            this.z400ImageItem.Index = 9;
            this.z400ImageItem.Text = "400%";
            this.z400ImageItem.Click += new System.EventHandler(this.zoomItem_Click);
            // 
            // z500ImageItem
            // 
            this.z500ImageItem.Index = 10;
            this.z500ImageItem.Text = "500%";
            this.z500ImageItem.Click += new System.EventHandler(this.zoomItem_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 11;
            this.menuItem8.Text = "-";
            // 
            // zoomInImageItem
            // 
            this.zoomInImageItem.Index = 12;
            this.zoomInImageItem.Shortcut = System.Windows.Forms.Shortcut.Ctrl8;
            this.zoomInImageItem.Text = "Zoom &In";
            this.zoomInImageItem.Click += new System.EventHandler(this.zoomInImageItem_Click);
            // 
            // zoomOutImageItem
            // 
            this.zoomOutImageItem.Index = 13;
            this.zoomOutImageItem.Shortcut = System.Windows.Forms.Shortcut.Ctrl7;
            this.zoomOutImageItem.Text = "Zoom &Out";
            this.zoomOutImageItem.Click += new System.EventHandler(this.zoomOutImageItem_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 14;
            this.menuItem11.Text = "-";
            // 
            // zoomFitImageItem
            // 
            this.zoomFitImageItem.Index = 15;
            this.zoomFitImageItem.Shortcut = System.Windows.Forms.Shortcut.Ctrl9;
            this.zoomFitImageItem.Text = "Fit to screen";
            this.zoomFitImageItem.Click += new System.EventHandler(this.zoomFitImageItem_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 2;
            this.menuItem10.Text = "-";
            // 
            // flipImageItem
            // 
            this.flipImageItem.Index = 3;
            this.flipImageItem.Text = "&Flip";
            this.flipImageItem.Click += new System.EventHandler(this.flipImageItem_Click);
            // 
            // mirrorItem
            // 
            this.mirrorItem.Index = 4;
            this.mirrorItem.Text = "&Mirror";
            this.mirrorItem.Click += new System.EventHandler(this.mirrorItem_Click);
            // 
            // rotateImageItem
            // 
            this.rotateImageItem.Index = 5;
            this.rotateImageItem.Text = "&Rotate 90 degree";
            this.rotateImageItem.Click += new System.EventHandler(this.rotateImageItem_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 6;
            this.menuItem3.Text = "-";
            // 
            // cropImageItem
            // 
            this.cropImageItem.Index = 7;
            this.cropImageItem.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.cropImageItem.Text = "Cro&p";
            this.cropImageItem.Click += new System.EventHandler(this.cropImageItem_Click);
            // 
            // filtersItem
            // 
            this.filtersItem.Index = 1;
            this.filtersItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.grayscaleColorFiltersItem,
            this.thresholdBinaryFiltersItem,
            this.edgeFiltersItem,
            this.resizeFiltersItem,
            this.rotateFiltersItem});
            this.filtersItem.MergeOrder = 1;
            this.filtersItem.Text = "Fi&lters";
            // 
            // grayscaleColorFiltersItem
            // 
            this.grayscaleColorFiltersItem.Index = 0;
            this.grayscaleColorFiltersItem.Text = "&Grayscale";
            this.grayscaleColorFiltersItem.Click += new System.EventHandler(this.grayscaleColorFiltersItem_Click);
            // 
            // thresholdBinaryFiltersItem
            // 
            this.thresholdBinaryFiltersItem.Index = 1;
            this.thresholdBinaryFiltersItem.Text = "&Threshold";
            this.thresholdBinaryFiltersItem.Click += new System.EventHandler(this.thresholdBinaryFiltersItem_Click);
            // 
            // edgeFiltersItem
            // 
            this.edgeFiltersItem.Index = 2;
            this.edgeFiltersItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.homogenityEdgeFiltersItem,
            this.differenceEdgeFiltersItem,
            this.sobelEdgeFiltersItem,
            this.cannyEdgeFiltersItem});
            this.edgeFiltersItem.Text = "&Edge detectors";
            // 
            // homogenityEdgeFiltersItem
            // 
            this.homogenityEdgeFiltersItem.Index = 0;
            this.homogenityEdgeFiltersItem.Text = "&Homogenity";
            this.homogenityEdgeFiltersItem.Click += new System.EventHandler(this.homogenityEdgeFiltersItem_Click);
            // 
            // differenceEdgeFiltersItem
            // 
            this.differenceEdgeFiltersItem.Index = 1;
            this.differenceEdgeFiltersItem.Text = "&Difference";
            this.differenceEdgeFiltersItem.Click += new System.EventHandler(this.differenceEdgeFiltersItem_Click);
            // 
            // sobelEdgeFiltersItem
            // 
            this.sobelEdgeFiltersItem.Index = 2;
            this.sobelEdgeFiltersItem.Text = "&Sobel";
            this.sobelEdgeFiltersItem.Click += new System.EventHandler(this.sobelEdgeFiltersItem_Click);
            // 
            // cannyEdgeFiltersItem
            // 
            this.cannyEdgeFiltersItem.Index = 3;
            this.cannyEdgeFiltersItem.Text = "";
            // 
            // resizeFiltersItem
            // 
            this.resizeFiltersItem.Index = 3;
            this.resizeFiltersItem.Text = "&Resize";
            this.resizeFiltersItem.Click += new System.EventHandler(this.resizeFiltersItem_Click);
            // 
            // rotateFiltersItem
            // 
            this.rotateFiltersItem.Index = 4;
            this.rotateFiltersItem.Text = "Ro&tate";
            this.rotateFiltersItem.Click += new System.EventHandler(this.rotateFiltersItem_Click);
            // 
            // FilterItem
            // 
            this.FilterItem.Index = 2;
            this.FilterItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.thresholdingSegment});
            this.FilterItem.MergeOrder = 1;
            this.FilterItem.Text = "Segment";
            // 
            // thresholdingSegment
            // 
            this.thresholdingSegment.Index = 0;
            this.thresholdingSegment.Text = "By Thresholding";
            this.thresholdingSegment.Click += new System.EventHandler(this.thresholdingSegment_Click);
            // 
            // resxultsMenu
            // 
            this.resxultsMenu.Index = 3;
            this.resxultsMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.csvResult});
            this.resxultsMenu.MergeOrder = 3;
            this.resxultsMenu.Text = "Results";
            // 
            // csvResult
            // 
            this.csvResult.Index = 0;
            this.csvResult.Text = "Generate PDF";
            this.csvResult.Click += new System.EventHandler(this.susanCometItem_Click);
            // 
            // lineVarienceMenu
            // 
            this.lineVarienceMenu.Index = 4;
            this.lineVarienceMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.drowLineMenu,
            this.analyseMenu});
            this.lineVarienceMenu.MergeOrder = 3;
            this.lineVarienceMenu.Text = "Line Variance Analysis";
            // 
            // drowLineMenu
            // 
            this.drowLineMenu.Index = 0;
            this.drowLineMenu.Text = "Draw Line";
            this.drowLineMenu.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // analyseMenu
            // 
            this.analyseMenu.Index = 1;
            this.analyseMenu.Text = "Analyze";
            this.analyseMenu.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // verificationMenu
            // 
            this.verificationMenu.Index = 5;
            this.verificationMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.checkerboardMenu,
            this.ditheringMenu,
            this.edgeVerienceMenu});
            this.verificationMenu.MergeOrder = 3;
            this.verificationMenu.Text = "Verification";
            // 
            // checkerboardMenu
            // 
            this.checkerboardMenu.Index = 0;
            this.checkerboardMenu.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.checkerboardMenu.Text = "Checkerboard";
            this.checkerboardMenu.Click += new System.EventHandler(this.checkerboardMenu_Click);
            // 
            // ditheringMenu
            // 
            this.ditheringMenu.Index = 1;
            this.ditheringMenu.Text = "Dithering";
            this.ditheringMenu.Click += new System.EventHandler(this.ditheringMenu_Click);
            // 
            // edgeVerienceMenu
            // 
            this.edgeVerienceMenu.Index = 2;
            this.edgeVerienceMenu.Text = "Edge Verience";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            //this.pictureBox1.Image = global::IPLab.Properties.Resources._477;
            this.pictureBox1.Location = new System.Drawing.Point(138, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(220, 225);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // ImageDoc
            // 
            this.AllowedStates = WeifenLuo.WinFormsUI.ContentStates.Document;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(528, 217);
            this.Controls.Add(this.pictureBox1);
            this.Menu = this.mainMenu;
            this.Name = "ImageDoc";
            this.Text = "Image";
            this.Load += new System.EventHandler(this.ImageDoc_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ImageDoc_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageDoc_MouseDown);
            this.MouseLeave += new System.EventHandler(this.ImageDoc_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageDoc_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageDoc_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        // Init the document
        private void Init()
        {
            // init components
            InitializeComponent();

            // form style
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);

            // init scroll bars
            this.AutoScroll = true;

            //Background worker
            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;

            UpdateSize();
        }

        // Execute command
        public void ExecuteCommand(ImageDocCommands cmd)
        {
            switch (cmd)
            {
                case ImageDocCommands.Clone:		// clone the image
                    Clone();
                    break;
                case ImageDocCommands.Crop:			// crop the image
                    Crop();
                    break;
                case ImageDocCommands.ZoomIn:		// zoom in
                    ZoomIn();
                    break;
                case ImageDocCommands.ZoomOut:		// zoom out
                    ZoomOut();
                    break;
                case ImageDocCommands.ZoomOriginal:	// original size
                    zoom = 1;
                    UpdateZoom();
                    break;
                case ImageDocCommands.FitToSize:	// fit to screen
                    FitToScreen();
                    break;
                case ImageDocCommands.Grayscale:	// grayscale
                    Grayscale();
                    break;
                case ImageDocCommands.Threshold:	// threshold
                    Threshold();
                    break;
                case ImageDocCommands.Resize:		// resize the image
                    ResizeImage();
                    break;
                case ImageDocCommands.Rotate:		// rotate the image
                    RotateImage();
                    break;

                case ImageDocCommands.Fourier:		// fourier transformation
                    ForwardFourierTransformation();
                    break;
            }
        }

        // Update document and notify client about changes
        private void UpdateNewImage()
        {
            // update size
            UpdateSize();
            // repaint
            Invalidate();

            // notify host
            if (DocumentChanged != null)
                DocumentChanged(this, null);
        }

        // Reload image from file
        public void Reload()
        {
            if (fileName != null)
            {
                try
                {
                    // load image
                    Bitmap newImage = (Bitmap)Bitmap.FromFile(fileName);

                    // Release current image
                    image.Dispose();
                    // set document image to just loaded
                    image = newImage;

                    // format image
                    AForge.Imaging.Image.FormatImage(ref image);
                }
                catch (Exception)
                {
                    throw new ApplicationException("Failed reloading image");
                }

                // update
                UpdateNewImage();
            }
        }

        // Center image in the document
        public void Center()
        {
            Rectangle rc = ClientRectangle;
            Point p = this.AutoScrollPosition;
            int width = (int)(this.width * zoom);
            int height = (int)(this.height * zoom);

            if (rc.Width < width)
                p.X = (width - rc.Width) >> 1;
            if (rc.Height < height)
                p.Y = (height - rc.Height) >> 1;

            this.AutoScrollPosition = p;
        }

        // Update document size 
        private void UpdateSize()
        {
            // image dimension
            width = image.Width;
            height = image.Height;

            // scroll bar size
            this.AutoScrollMinSize = new Size((int)(width * zoom), (int)(height * zoom));
        }

        // Paint image
        protected override void OnPaint(PaintEventArgs e)
        {
            if (image != null)
            {
                Graphics g = e.Graphics;
                Rectangle rc = ClientRectangle;
                Pen pen = new Pen(Color.FromArgb(0, 0, 0));

                int width = (int)(this.width * zoom);
                int height = (int)(this.height * zoom);
                int x = (rc.Width < width) ? this.AutoScrollPosition.X : (rc.Width - width) / 2;
                int y = (rc.Height < height) ? this.AutoScrollPosition.Y : (rc.Height - height) / 2;

                // draw rectangle around the image
                g.DrawRectangle(pen, x - 1, y - 1, width + 1, height + 1);

                // set nearest neighbor interpolation to avoid image smoothing
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                // draw image
                g.DrawImage(image, x, y, width, height);

                pen.Dispose();
            }
        }

        // Mouse click
        protected override void OnClick(EventArgs e)
        {
            Focus();
        }

        // Apply filter on the image
        private void ApplyFilter(IFilter filter)
        {
            try
            {
                // set wait cursor
                this.Cursor = Cursors.WaitCursor;

                // apply filter to the image
                Bitmap newImage = filter.Apply(image);

                if (host.CreateNewDocumentOnChange)
                {
                    // open new image in new document
                    host.NewDocument(newImage);
                }
                else
                {
                    if (host.RememberOnChange)
                    {
                        // backup current image
                        if (backup != null)
                            backup.Dispose();

                        backup = image;
                    }
                    else
                    {
                        // release current image
                        image.Dispose();
                    }

                    image = newImage;

                    // update
                    UpdateNewImage();
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Selected filter can not be applied to the image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // restore cursor
                this.Cursor = Cursors.Default;
            }
        }

        // on "Image" item popup
        private void imageItem_Popup(object sender, System.EventArgs e)
        {
            this.backImageItem.Enabled = (backup != null);
            this.cropImageItem.Checked = cropping;
        }

        // Restore image to previous
        private void backImageItem_Click(object sender, System.EventArgs e)
        {
            if (backup != null)
            {
                // release current image
                image.Dispose();
                // restore
                image = backup;
                backup = null;

                // update
                UpdateNewImage();
            }
        }

        // Clone the image
        private void Clone()
        {
            if (host != null)
            {
                Bitmap clone = AForge.Imaging.Image.Clone(image);

                if (!host.NewDocument(clone))
                {
                    clone.Dispose();
                }
            }
        }

        // On "Image->Clone" item click
        private void cloneImageItem_Click(object sender, System.EventArgs e)
        {
            Clone();
        }

        // Update zoom factor
        private void UpdateZoom()
        {
            this.AutoScrollMinSize = new Size((int)(width * zoom), (int)(height * zoom));
            this.Invalidate();

            // notify host
            if (ZoomChanged != null)
                ZoomChanged(this, null);
        }

        // Zoom image
        private void zoomItem_Click(object sender, System.EventArgs e)
        {
            // get menu item text
            String t = ((MenuItem)sender).Text;
            // parse it`s value
            int i = int.Parse(t.Remove(t.Length - 1, 1));
            // calc zoom factor
            zoom = (float)i / 100;

            UpdateZoom();
        }

        // Zoom In image
        private void ZoomIn()
        {
            float z = zoom * 1.5f;

            if (z <= 10)
            {
                zoom = z;
                UpdateZoom();
            }
        }

        // On "Image->Zoom->Zoom In" item click
        private void zoomInImageItem_Click(object sender, System.EventArgs e)
        {
            ZoomIn();
        }

        // Zoom Out image
        private void ZoomOut()
        {
            float z = zoom / 1.5f;

            if (z >= 0.05)
            {
                zoom = z;
                UpdateZoom();
            }
        }

        // On "Image->Zoom->Zoom out" item click
        private void zoomOutImageItem_Click(object sender, System.EventArgs e)
        {
            ZoomOut();
        }

        // Fit to size
        private void FitToScreen()
        {
            Rectangle rc = ClientRectangle;

            zoom = Math.Min((float)rc.Width / (width + 2), (float)rc.Height / (height + 2));

            UpdateZoom();
        }

        // On "Image->Zoom->Fit To Screen" item click
        private void zoomFitImageItem_Click(object sender, System.EventArgs e)
        {
            FitToScreen();
        }

        // Flip image
        private void flipImageItem_Click(object sender, System.EventArgs e)
        {
            image.RotateFlip(RotateFlipType.RotateNoneFlipY);

            Invalidate();
        }

        // Mirror image
        private void mirrorItem_Click(object sender, System.EventArgs e)
        {
            image.RotateFlip(RotateFlipType.RotateNoneFlipX);

            Invalidate();
        }

        // Rotate image 90 degree
        private void rotateImageItem_Click(object sender, System.EventArgs e)
        {
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);

            // update
            UpdateNewImage();
        }

        // Invert image
        private void invertColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new Invert());
        }

        // Rotatet colors
        private void rotateColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new RotateChannels());
        }

        // Sepia image
        private void sepiaColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new Sepia());
        }

        // Grayscale image
        private void Grayscale()
        {
            if (image.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                MessageBox.Show("The image is already grayscale", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ApplyFilter(new GrayscaleBT709());
        }

        // On "Filter->Color->Grayscale"
        private void grayscaleColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            Grayscale();
        }

        // Converts grayscale image to RGB
        private void toRgbColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            if (image.PixelFormat == PixelFormat.Format24bppRgb)
            {
                MessageBox.Show("The image is already RGB", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ApplyFilter(new GrayscaleToRGB());
        }

        // Remove green and blue channels
        private void redColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new ChannelFiltering(new IntRange(0, 255), new IntRange(0, 0), new IntRange(0, 0)));
        }

        // Remove red and blue channels
        private void greenColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new ChannelFiltering(new IntRange(0, 0), new IntRange(0, 255), new IntRange(0, 0)));
        }

        // Remove red and green channels
        private void blueColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new ChannelFiltering(new IntRange(0, 0), new IntRange(0, 0), new IntRange(0, 255)));
        }

        // Remove green channel
        private void cyanColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new ChannelFiltering(new IntRange(0, 0), new IntRange(0, 255), new IntRange(0, 255)));
        }

        // Remove green channel
        private void magentaColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new ChannelFiltering(new IntRange(0, 255), new IntRange(0, 0), new IntRange(0, 255)));
        }

        // Remove blue channel
        private void yellowColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new ChannelFiltering(new IntRange(0, 255), new IntRange(0, 255), new IntRange(0, 0)));
        }

        // Extract red channel of image
        private void extractRedColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new ExtractChannel(RGB.R));
        }

        // Extract green channel of image
        private void extractGreenColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new ExtractChannel(RGB.G));
        }

        // Extract blue channel of image
        private void extractRedBlueFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new ExtractChannel(RGB.B));
        }

        // Replace red channel
        private void replaceRedColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            // check pixel format
            if (image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                MessageBox.Show("Channels replacement can be applied to RGB images only", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Bitmap channelImage = host.GetImage(this, "Select an image which will replace the red channel in the current image", new Size(width, height), PixelFormat.Format8bppIndexed);

            if (channelImage != null)
                ApplyFilter(new ReplaceChannel(RGB.R, channelImage));
        }

        // Replace green channel
        private void replaceGreenColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            // check pixel format
            if (image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                MessageBox.Show("Channels replacement can be applied to RGB images only", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Bitmap channelImage = host.GetImage(this, "Select an image which will replace the green channel in the current image", new Size(width, height), PixelFormat.Format8bppIndexed);

            if (channelImage != null)
                ApplyFilter(new ReplaceChannel(RGB.G, channelImage));
        }

        // Replace blue channel
        private void replaceBlueColorFiltersItem_Click(object sender, System.EventArgs e)
        {
            // check pixel format
            if (image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                MessageBox.Show("Channels replacement can be applied to RGB images only", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Bitmap channelImage = host.GetImage(this, "Select an image which will replace the blue channel in the current image", new Size(width, height), PixelFormat.Format8bppIndexed);

            if (channelImage != null)
                ApplyFilter(new ReplaceChannel(RGB.B, channelImage));
        }



        // Extract Y channel of YCbCr color space
        private void extracYFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new YCbCrExtractChannel(YCbCr.YIndex));
        }

        // Extract Cb channel of YCbCr color space
        private void extracCbFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new YCbCrExtractChannel(YCbCr.CbIndex));
        }

        // Extract Cr channel of YCbCr color space
        private void extracCrFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new YCbCrExtractChannel(YCbCr.CrIndex));
        }

        // Replace Y channel of YCbCr color space
        private void replaceYFiltersItem_Click(object sender, System.EventArgs e)
        {
            // check pixel format
            if (image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                MessageBox.Show("Channels replacement can be applied to RGB images only", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Bitmap channelImage = host.GetImage(this, "Select an image which will replace the Y channel in the current image", new Size(width, height), PixelFormat.Format8bppIndexed);

            if (channelImage != null)
                ApplyFilter(new YCbCrReplaceChannel(YCbCr.YIndex, channelImage));
        }

        // Replace Cb channel of YCbCr color space
        private void replaceCbFiltersItem_Click(object sender, System.EventArgs e)
        {
            // check pixel format
            if (image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                MessageBox.Show("Channels replacement can be applied to RGB images only", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Bitmap channelImage = host.GetImage(this, "Select an image which will replace the Cb channel in the current image", new Size(width, height), PixelFormat.Format8bppIndexed);

            if (channelImage != null)
                ApplyFilter(new YCbCrReplaceChannel(YCbCr.CbIndex, channelImage));
        }

        // Replace Cr channel of YCbCr color space
        private void replaceCrFiltersItem_Click(object sender, System.EventArgs e)
        {
            // check pixel format
            if (image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                MessageBox.Show("Channels replacement can be applied to RGB images only", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Bitmap channelImage = host.GetImage(this, "Select an image which will replace the Cr channel in the current image", new Size(width, height), PixelFormat.Format8bppIndexed);

            if (channelImage != null)
                ApplyFilter(new YCbCrReplaceChannel(YCbCr.CrIndex, channelImage));
        }

        // Threshold binarization
        private void Threshold()
        {
            ThresholdForm form = new ThresholdForm();

            // set image to preview
            form.Image = image;

            if (form.ShowDialog() == DialogResult.OK)
            {
                ApplyFilter(form.Filter);
            }
        }

        // On "Filters->Binarization->Threshold" menu item click
        private void thresholdBinaryFiltersItem_Click(object sender, System.EventArgs e)
        {
            Threshold();
        }

        // Threshold binarization with carry
        private void thresholdCarryBinaryFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new ThresholdWithCarry());
        }

        // Ordered dithering
        private void orderedDitherBinaryFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new OrderedDithering());
        }

        // Bayer ordered dithering
        private void bayerDitherBinaryFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new BayerDithering());
        }

        // Binarization using Floyd-Steinverg dithering algorithm
        private void floydBinaryFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new FloydSteinbergDithering());
        }

        // Binarization using Burkes dithering algorithm
        private void burkesBinaryFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new BurkesDithering());
        }

        // Binarization using Stucki dithering algorithm
        private void stuckiBinaryFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new StuckiDithering());
        }

        // Binarization using Jarvis, Judice and Ninke dithering algorithm
        private void jarvisBinaryFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new JarvisJudiceNinkeDithering());
        }

        // Binarization using Sierra dithering algorithm
        private void sierraBinaryFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new SierraDithering());
        }

        // Binarization using Stevenson and Arce dithering algorithm
        private void stevensonBinaryFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new StevensonArceDithering());
        }

        // Threshold using Simple Image Statistics
        private void sisThresholdBinaryFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new SISThreshold());
        }

        // Errosion (Mathematical Morphology)
        private void erosionMorphologyFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new Erosion());
        }

        // Dilatation (Mathematical Morphology)
        private void dilatationMorphologyFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new Dilatation());
        }

        // Opening (Mathematical Morphology)
        private void openingMorphologyFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new Opening());
        }

        // Closing (Mathematical Morphology)
        private void closingMorphologyFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new Closing());
        }



        // Mean
        private void meanConvolutionFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new Mean());
        }

        // Blur
        private void blurConvolutionFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new Blur());
        }


        // Sharpen
        private void sharpenConvolutionFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new Sharpen());
        }

        // Edges
        private void edgesConvolutionFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new Edges());
        }


        // Merge two images
        private void mergeTwosrcFiltersItem_Click(object sender, System.EventArgs e)
        {
            Bitmap overlayImage = host.GetImage(this, "Select an image to merge with the curren image", new Size(-1, -1), image.PixelFormat);

            if (overlayImage != null)
                ApplyFilter(new Merge(overlayImage));
        }

        // Intersect
        private void intersectTwosrcFiltersItem_Click(object sender, System.EventArgs e)
        {
            Bitmap overlayImage = host.GetImage(this, "Select an image to intersect with the curren image", new Size(-1, -1), image.PixelFormat);

            if (overlayImage != null)
                ApplyFilter(new Intersect(overlayImage));
        }

        // Add
        private void addTwosrcFiltersItem_Click(object sender, System.EventArgs e)
        {
            Bitmap overlayImage = host.GetImage(this, "Select an image to add to the curren image", new Size(-1, -1), image.PixelFormat);

            if (overlayImage != null)
                ApplyFilter(new Add(overlayImage));
        }

        // Subtract
        private void subtractTwosrcFiltersItem_Click(object sender, System.EventArgs e)
        {
            Bitmap overlayImage = host.GetImage(this, "Select an image to subtract from the curren image", new Size(-1, -1), image.PixelFormat);

            if (overlayImage != null)
                ApplyFilter(new Subtract(overlayImage));
        }

        // Difference
        private void differenceTwosrcFiltersItem_Click(object sender, System.EventArgs e)
        {
            Bitmap overlayImage = host.GetImage(this, "Select an image to get difference with the curren image", new Size(width, height), image.PixelFormat);

            if (overlayImage != null)
                ApplyFilter(new Difference(overlayImage));
        }

        // Move towards
        private void moveTowardsTwosrcFiltersItem_Click(object sender, System.EventArgs e)
        {
            Bitmap overlayImage = host.GetImage(this, "Select an image to which the curren image will be moved", new Size(width, height), image.PixelFormat);

            if (overlayImage != null)
                ApplyFilter(new MoveTowards(overlayImage, 10));
        }


        // Homogenity edge detector
        private void homogenityEdgeFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new HomogenityEdgeDetector());
        }

        // Difference edge detector
        private void differenceEdgeFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new DifferenceEdgeDetector());
        }

        // Sobel edge detector
        private void sobelEdgeFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new SobelEdgeDetector());
        }



        // Conservative smoothing
        private void conservativeSmoothingFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new ConservativeSmoothing());
        }



        // Random jitter filter
        private void jitterFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new Jitter(1));
        }



        // Simple skeletonization
        private void simpleSkeletonizationFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new SimpleSkeletonization());
        }


        // Conected components labeling
        private void labelingFiltersItem_Click(object sender, System.EventArgs e)
        {
            if (image.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                MessageBox.Show("Connected components labeling can be applied to binary images only", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ApplyFilter(new ConnectedComponentsLabeling());
        }

        // Extract separate blobs
        private void blobExtractorFiltersItem_Click(object sender, System.EventArgs e)
        {
            if (image.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                MessageBox.Show("Blob extractor can be applied to binary images only", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            BlobCounter blobCounter = new BlobCounter(image);
            Blob[] blobs = blobCounter.GetObjects(image);

            foreach (Blob blob in blobs)
            {
                host.NewDocument(blob.Image);
            }
        }

        // Resize the image
        private void ResizeImage()
        {
            ResizeForm form = new ResizeForm();

            form.OriginalSize = new Size(width, height);

            if (form.ShowDialog() == DialogResult.OK)
            {
                ApplyFilter(form.Filter);
            }
        }

        // On "Filters->Resize" menu item click
        private void resizeFiltersItem_Click(object sender, System.EventArgs e)
        {
            ResizeImage();
        }

        // Rotate the image
        private void RotateImage()
        {
            RotateForm form = new RotateForm();

            if (form.ShowDialog() == DialogResult.OK)
            {
                ApplyFilter(form.Filter);
            }
        }

        // On "Filters->Rotate" menu item click
        private void rotateFiltersItem_Click(object sender, System.EventArgs e)
        {
            RotateImage();
        }


        // Median filter
        private void medianFiltersItem_Click(object sender, System.EventArgs e)
        {
            ApplyFilter(new Median());
        }


        // Fourier transformation
        private void ForwardFourierTransformation()
        {
            System.Diagnostics.Debug.WriteLine((int)FourierTransform.Direction.Forward);
            System.Diagnostics.Debug.WriteLine((int)FourierTransform.Direction.Backward);

            if ((!AForge.Math.Tools.IsPowerOf2(width)) ||
                (!AForge.Math.Tools.IsPowerOf2(height)))
            {
                MessageBox.Show("Fourier trasformation can be applied to an image with width and height of power of 2", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ComplexImage cImage = ComplexImage.FromBitmap(image);

            cImage.ForwardFourierTransform();
            host.NewDocument(cImage);
        }

        // On "Filters->Fourier Transformation" click
        private void fourierFiltersItem_Click(object sender, System.EventArgs e)
        {
            ForwardFourierTransformation();
        }

        // Calculate image and screen coordinates of the point
        private void GetImageAndScreenPoints(Point point, out Point imgPoint, out Point screenPoint)
        {
            Rectangle rc = this.ClientRectangle;
            int width = (int)(this.width * zoom);
            int height = (int)(this.height * zoom);
            int x = (rc.Width < width) ? this.AutoScrollPosition.X : (rc.Width - width) / 2;
            int y = (rc.Height < height) ? this.AutoScrollPosition.Y : (rc.Height - height) / 2;

            int ix = Math.Min(Math.Max(x, point.X), x + width - 1);
            int iy = Math.Min(Math.Max(y, point.Y), y + height - 1);

            ix = (int)((ix - x) / zoom);
            iy = (int)((iy - y) / zoom);

            // image point
            imgPoint = new Point(ix, iy);
            // screen point
            screenPoint = this.PointToScreen(new Point((int)(ix * zoom + x), (int)(iy * zoom + y)));
        }

        // Normalize points so, that pt1 becomes top-left point of rectangle
        // and pt2 becomes right-bottom
        private void NormalizePoints(ref Point pt1, ref Point pt2)
        {
            Point t1 = pt1;
            Point t2 = pt2;

            pt1.X = Math.Min(t1.X, t2.X);
            pt1.Y = Math.Min(t1.Y, t2.Y);
            pt2.X = Math.Max(t1.X, t2.X);
            pt2.Y = Math.Max(t1.Y, t2.Y);
        }

        // Draw selection rectangle
        private void DrawSelectionFrame(Graphics g)
        {
            Point sp = startW;
            Point ep = endW;

            // Normalize points
            NormalizePoints(ref sp, ref ep);
            // Draw reversible frame
            ControlPaint.DrawReversibleFrame(new Rectangle(sp.X, sp.Y, ep.X - sp.X + 1, ep.Y - sp.Y + 1), Color.White, FrameStyle.Dashed);
        }

        // Crop the image
        private void Crop()
        {
            if (!cropping)
            {
                // turn on
                cropping = true;
                this.Cursor = Cursors.Cross;

            }
            else
            {
                // turn off
                cropping = false;
                this.Cursor = Cursors.Default;
            }
        }

        // Dithering - Select uniform color region the image
        private void UniformCrop()
        {
            System.Drawing.Bitmap ditheringImageCheck = ditheringImage;
            MainForm mainForm = this.TopLevelControl as MainForm;
            //form.disableToolbars();
            mainForm.visibleTrueToolbars();

            if (!cropping)
            {
                // turn on
                cropping = true;
                this.Cursor = Cursors.Cross;

            }
            else
            {
                // turn off
                cropping = false;
                this.Cursor = Cursors.Default;
            }
        }

        // On "Image->Crop" - turn on/off cropping mode
        private void cropImageItem_Click(object sender, System.EventArgs e)
        {
            Crop();
        }

        // On mouse down
        private void ImageDoc_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // turn off cropping mode
                if (!dragging)
                {
                    cropping = false;
                    this.Cursor = Cursors.Default;
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (cropping)
                {
                    // start dragging
                    dragging = true;
                    // set mouse capture
                    this.Capture = true;

                    // get selection start point
                    GetImageAndScreenPoints(new Point(e.X, e.Y), out start, out startW);

                    // end point is the same as start
                    end = start;
                    endW = startW;

                    // draw frame
                    Graphics g = this.CreateGraphics();
                    DrawSelectionFrame(g);
                    g.Dispose();
                }
                else if (drawingLine)
                {
                    Point end;
                    GetImageAndScreenPoints(new Point(e.X, e.Y), out imageStart, out end);
                    dragging = true;
                }
            }
        }

        // On mouse up
        private void ImageDoc_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (dragging && drawingLine)
            {
                dragging = drawingLine = false;
                this.Capture = false;
                // set default mouse pointer
                this.Cursor = Cursors.Default;

                // erase frame
                Graphics g = this.CreateGraphics();
                DrawSelectionFrame(g);
                g.Dispose();

                Point end;
                GetImageAndScreenPoints(new Point(e.X, e.Y), out imageEnd, out end);
                lineLength = Math.Sqrt(Math.Pow(imageEnd.X - imageStart.X, 2) + Math.Pow(imageEnd.Y - imageStart.Y, 2));
                dragging = false;
                DrawLineInt(image, imageStart.X, imageEnd.X, imageStart.Y, imageEnd.Y);
                // normalize start and end points
                NormalizePoints(ref start, ref end);
            }
            else if (dragging)
            {
                // stop dragging and cropping
                dragging = cropping = false;
                // release capture
                this.Capture = false;
                // set default mouse pointer
                this.Cursor = Cursors.Default;

                // erase frame
                Graphics g = this.CreateGraphics();
                DrawSelectionFrame(g);
                g.Dispose();

                // normalize start and end points
                NormalizePoints(ref start, ref end);

                // crop the image
                ApplyFilter(new Crop(new Rectangle(start.X, start.Y, end.X - start.X + 1, end.Y - start.Y + 1)));
            }

        }

        // On mouse move
        private void ImageDoc_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (dragging && !drawingLine)
            {

                Graphics g = this.CreateGraphics();

                // erase frame
                DrawSelectionFrame(g);

                // get selection end point
                GetImageAndScreenPoints(new Point(e.X, e.Y), out end, out endW);

                // draw frame
                DrawSelectionFrame(g);

                g.Dispose();

                if (SelectionChanged != null)
                {
                    Point sp = start;
                    Point ep = end;

                    // normalize start and end points
                    NormalizePoints(ref sp, ref ep);

                    SelectionChanged(this, new SelectionEventArgs(
                        sp, new Size(ep.X - sp.X + 1, ep.Y - sp.Y + 1)));
                }
            }
            else
            {
                if (MouseImagePosition != null)
                {
                    Rectangle rc = this.ClientRectangle;
                    int width = (int)(this.width * zoom);
                    int height = (int)(this.height * zoom);
                    int x = (rc.Width < width) ? this.AutoScrollPosition.X : (rc.Width - width) / 2;
                    int y = (rc.Height < height) ? this.AutoScrollPosition.Y : (rc.Height - height) / 2;

                    if ((e.X >= x) && (e.Y >= y) &&
                        (e.X < x + width) && (e.Y < y + height))
                    {
                        // mouse is over the image
                        MouseImagePosition(this, new SelectionEventArgs(
                            new Point((int)((e.X - x) / zoom), (int)((e.Y - y) / zoom))));
                    }
                    else
                    {
                        // mouse is outside image region
                        MouseImagePosition(this, new SelectionEventArgs(new Point(-1, -1)));
                    }
                }
            }
        }

        // On mouse leave
        private void ImageDoc_MouseLeave(object sender, System.EventArgs e)
        {
            if ((!dragging) && (MouseImagePosition != null))
            {
                MouseImagePosition(this, new SelectionEventArgs(new Point(-1, -1)));
            }
        }

        #region EdgeVariation

        /// <summary>
        /// calculate the depth variation according to the measured depth value.
        /// </summary>
        /// <returns></returns>
        private double getVariationofDepth()
        {
            List<int> depths = getAllDepthsToBorder();
            double variance = Variance(depths, Mean(depths));
            //MessageBox.Show(this, variance.ToString());
            //writeResult(variance, "LazerJet 600");
            makeOutput(variance);
            return variance;
        }

        /// <summary>
        /// show the results in a message and write the metadata to the matadata file for final result.
        /// </summary>
        /// <param name="varience"></param>
        private void makeOutput(double varience)
        {
            double precentage = ((15 - varience) / 14) * 100;
            if (precentage < 0)
            {
                precentage = 0;
            }
            MainForm mainForm = this.TopLevelControl as MainForm;
            mainForm.saveEdgeverienceTxt("According to the analysis this signature is " + precentage + "% genuin.");
            List<string> metadataList= new List<string>();
            metadataList.Add(precentage.ToString());
            writeToMetadataFile(metadataList, "edg");
            MessageBox.Show(this, "According to the analysis this signature is " + (100 - precentage) + "% forged.");
        }

        /// <summary>
        /// Use this method to store rresults of each analysis.
        /// </summary>
        /// <param name="metadataList">list of string containing data. last element should only contain a double value </param>
        /// <param name="sender">who is sending the data. dit-Dithering, edg-edge analysis, che- checker board analaysis</param>
        private void writeToMetadataFile(List<string> metadataList, string sender)
        {
            if (sender.Equals("dit"))
            {
                //check
                tempMetadataList[0] = metadataList[0];
                tempMetadataList[1] = metadataList[1];
                tempMetadataList[2] = metadataList[2];
                tempMetadataList[3] = metadataList[3];
            }
            else if (sender.Equals("edg"))
            {
                tempMetadataList[4] = metadataList[0];
            }
            else if (sender.Equals("che"))
            {
                tempMetadataList[5] = metadataList[0];
                tempMetadataList[6] = metadataList[1];
                tempMetadataList[7] = metadataList[2];
                tempMetadataList[8] = metadataList[3];
                tempMetadataList[9] = metadataList[4];
            }

            foreach (string data in tempMetadataList)
            {
                if (data.Equals("null"))
                {
                    return;
                }
            }

            using (StreamWriter sw = File.AppendText(Properties.Resources.workingPath + @"MetadataText.txt"))
            {
                sw.WriteLine("This");
                sw.WriteLine("is Extra");
                sw.WriteLine("Text");
            }

            tempMetadataList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="type"></param>
        private void writeResult(double result, string type)
        {
            string path = type + ".txt";
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(result);
                }
            }

            // This text is always added, making the file longer over time 
            // if it is not deleted. 
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(result);
            }

        }

        /// <summary>
        /// calculate the varience of a given value list
        /// </summary>
        /// <param name="values">values which are to be used to calculate the varience</param>
        /// <param name="mean">mean value of the value list</param>
        /// <returns>varience of the value list</returns>
        public static double Variance(List<int> values, double mean)
        {
            double variance = 0;

            for (int i = 0; i < values.Count; i++)
            {
                variance += Math.Pow((values[i] - mean), 2);
            }

            //int n = end - start;
            //if (start > 0) n -= 1;

            return variance / values.Count;
        }

        /// <summary>
        /// calculate the mean of a given value list
        /// </summary>
        /// <param name="values">list of values to be calculate the mean</param>
        /// <returns>mean of the value list</returns>
        public static double Mean(List<int> values)
        {
            double s = values.Sum();

            return s / values.Count;
        }

        private List<int> getLengthToBorder()
        {
            List<int> lengths = new List<int>();


            int yValueofLine = getYvalueofLine(), count = 0;

            for (int i = 0; i < image.Width; i++)
            {
                int j = 0;
                if (rgbIsInRange(image.GetPixel(i, yValueofLine), Color.Red, 50))
                {
                    while (rgbIsInRange(image.GetPixel(i, yValueofLine + j), Color.Red, 50))
                    {
                        j++;
                    }
                    Color temp = image.GetPixel(i, yValueofLine + j);
                    while (rgbIsInRange(image.GetPixel(i, yValueofLine + j + 1), temp, 50))
                    {
                        count++;
                        j++;
                    }
                }
                lengths.Add(count);
            }

            return lengths;
        }

        /// <summary>
        /// find distance between line and the edge of the signature.
        /// </summary>
        /// <returns>list containing all the depth information</returns>
        private List<int> getAllDepthsToBorder()
        {
            List<int> depths = new List<int>();
            int length = (int)lineLength;
            Color referenceColor = image.GetPixel(LineStart.X, LineStart.Y);
            for (int i = LineStart.X; i < (LineStart.X + length); i++)
            {
                int count = 0;
                for (int j = LineStart.Y; j >= 0; j--)
                {
                    if (i < image.Width && j < image.Height)
                    {
                        if (referenceColor == image.GetPixel(i, j))
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                depths.Add(count);
            }
            return depths;
        }

        /// <summary>
        /// find the y cordinate of the drawn line
        /// </summary>
        /// <returns>y cordinate of the drawn line</returns>
        private int getYvalueofLine()
        {
            int y = 0;
            for (int i = 0; i < image.Height; i++)
            {
                if (rgbIsInRange(image.GetPixel(image.Width / 2, i), Color.Red, 50))
                {
                    y = i;
                    break;
                }
            }
            return y;
        }

        /// <summary>
        /// find the x and y cordinates of starting point of the drawn line.
        /// </summary>
        /// <returns>starting point of the line</returns>
        private Point getLineStartPoint()
        {
            Point point = new Point(0, 0);
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    if (rgbIsInRange(image.GetPixel(j, i), Color.Red, 20))
                    {
                        point = new Point(j, i);
                        return point;
                    }
                }
            }

            return point;
        }

        /// <summary>
        /// calculate wheather the two colors are in the same region or not
        /// </summary>
        /// <param name="reference">reference color</param>
        /// <param name="color">color which will check with reference color</param>
        /// <param name="tolerence">tolerence respect to the reference color</param>
        /// <returns>returns true if two colors are in same region, else return false</returns>
        private static bool rgbIsInRange(Color reference, Color color, int tolerence)
        {
            if (((reference.A - tolerence) < color.A) && (color.A < (reference.A + tolerence)) &&
                ((reference.R - tolerence) < color.R) && (color.R < (reference.R + tolerence)) &&
                ((reference.G - tolerence) < color.G) && (color.G < (reference.G + tolerence)) &&
                ((reference.B - tolerence) < color.B) && (color.B < (reference.B + tolerence)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// draw line
        /// </summary>
        /// <param name="bmp">image where the line is suppose to be drawn</param>
        /// <param name="x1">starting point x cordinates</param>
        /// <param name="x2">end point x cordinates</param>
        /// <param name="y1">starting point y cordinates</param>
        /// <param name="y2">end point x cordinates</param>
        private void DrawLineInt(Bitmap bmp, int x1, int x2, int y1, int y2)
        {
            Pen blackPen = new Pen(Color.Red, 2);

            // Draw line to screen.
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.DrawLine(blackPen, x1, y1, x2, y2);
            }
            UpdateNewImage();
            RotateImageBilinear(getAngleofLine() * 180 / Math.PI);
            LineStart = getLineStartPoint();

        }

        /// <summary>
        /// calculate the angle of the line.
        /// </summary>
        /// <returns>angle of the line.</returns>
        private double getAngleofLine()
        {
            return Math.Atan(((imageStart.Y - imageEnd.Y) * 1.0) / (imageStart.X - imageEnd.X));
        }

        /// <summary>
        /// rotate image using bilinear meathod
        /// </summary>
        /// <param name="angle">angle of the rotation</param>
        /// <returns>rotated image.</returns>
        private Bitmap RotateImageBilinear(double angle)
        {
            RotateBilinear rotateFilter = new RotateBilinear(angle, false);
            image = rotateFilter.Apply(image);
            UpdateNewImage();
            return image;
        }

        /// <summary>
        /// Gets the list of cornes
        /// <param name="threshold">The threshold.</param>
        private void susanCometItem_Click(object sender, EventArgs e)
        {
            //ResultForm form = new ResultForm();
            //form.ShowDialog();

            BackgroundWorker worker = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);


            pictureBox1.Visible = true;
            this.Enabled = false;

            bw.RunWorkerAsync();

        }

        /// <summary>
        /// runs when the background work is end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox1.Visible = false;
            this.Enabled = true;
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        /// <summary>
        /// create the report in a background thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Report report = new Report();
            report.generateReport();
        }

        /// <summary>
        /// Draw line.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Cross;
            drawingLine = true;
        }

        private void ImageDoc_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                tempMetadataList.Add("null");
            }
        }

        #endregion

        #region Tharindu Edit

        /// <summary>
        /// Handles the Click event of the thresholdingSegment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void thresholdingSegment_Click(object sender, EventArgs e)
        {
            Bitmap map = AForge.Imaging.Image.Clone(image);

            ThresholdForm form = new ThresholdForm();

            // set image to preview
            form.Image = image;

            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // set wait cursor
                    this.Cursor = Cursors.WaitCursor;

                    // apply filter to the image
                    Bitmap newImage = form.Filter.Apply(map);

                    if (host.CreateNewDocumentOnChange)
                    {
                        // open new image in new document
                        host.NewDocument(newImage);
                    }
                    else
                    {
                        //image = newImage;
                        createCroppedImage(map, newImage);

                        if (host.RememberOnChange)
                        {
                            // backup current image
                            if (backup != null)
                                backup.Dispose();

                            backup = image;
                        }
                                                

                        // update
                        UpdateNewImage();
                    }
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Selected filter can not be applied to the image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // restore cursor
                    this.Cursor = Cursors.Default;
                }


            }
        }

        /// <summary>
        /// Creates the cropped image.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="newImage">The new image.</param>
        /// <remarks></remarks>
        private void createCroppedImage(Bitmap map, Bitmap newImage)
        {
            //create cropped image
            Bitmap newmap = new Bitmap(image.Width, image.Height);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    if (newImage.GetPixel(i, j).ToArgb() != -1)
                    {
                        newmap.SetPixel(i, j, map.GetPixel(i, j));
                    }
                }
            }
            image.Dispose();
            image = newmap;
        }

        /// <summary>
        /// Handles the Click event of the edgeSegment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void edgeSegment_Click(object sender, EventArgs e)
        {
            //ApplyFilter (new HomogenityEdgeDetector());    

            Bitmap map = AForge.Imaging.Image.Clone(image);

            // apply filter to the image
            Bitmap newImage = new HomogenityEdgeDetector().Apply(map);

            if (host.CreateNewDocumentOnChange)
            {
                // open new image in new document
                host.NewDocument(newImage);
            }
            else
            {
                if (host.RememberOnChange)
                {
                    // backup current image
                    if (backup != null)
                        backup.Dispose();

                    backup = image;
                }
                else
                {
                    // release current image
                    image.Dispose();
                }

                //Create the image
                //image = newImage;
                Bitmap edgeImage = getItemsWithinEdges(newImage, map, 50, false);

                createCroppedImage(map, newImage);

                // update
                UpdateNewImage();
            }
        }

        /// <summary>
        /// Gets the items within edges.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="threshold">The threshold.</param>
        /// <param name="getLargestBlobe">if set to <c>true</c> [get largest blobe].</param>
        /// <returns></returns>
        private static Bitmap getItemsWithinEdges(Bitmap edgeImage, Bitmap originalImage, int threshold, bool getLargestBlobe)
        {
            if (originalImage != null)
            {
                try
                {
                    Bitmap edges = new Bitmap(originalImage.Width, originalImage.Height);
                    Bitmap retVal = new Bitmap(originalImage.Width, originalImage.Height);
                    Color white = Color.White;
                    Color black = Color.Black;


                    for (int i = 0; i < originalImage.Height; i++)
                    {
                        for (int j = 1; j < originalImage.Width; j++)
                        {
                            Color temp = edgeImage.GetPixel(j, i);
                            if (temp.R == 0 && temp.G == 0 && temp.B == 0)
                            {
                                edges.SetPixel(j, i, black);
                            }
                            else
                            {
                                edges.SetPixel(j, i, white);
                                getConnectedItems(originalImage, edges, j, i, threshold);
                            }

                            //to get more smooth image
                            if (j + 1 < originalImage.Width && edges.GetPixel(j + 1, i).ToArgb() == white.ToArgb())
                            {
                                getConnectedItems(originalImage, edges, j + 1, i, threshold);
                            }
                            if (j - 1 > 0 && edges.GetPixel(j - 1, i).ToArgb() == white.ToArgb())
                            {
                                getConnectedItems(originalImage, edges, j - 1, i, threshold);
                            }
                            if (i + 1 < originalImage.Height && edges.GetPixel(j, i + 1).ToArgb() == white.ToArgb())
                            {
                                getConnectedItems(originalImage, edges, j, i + 1, threshold);
                            }
                            if (i - 1 >= 0 && edges.GetPixel(j, i - 1).ToArgb() == white.ToArgb())
                            {
                                getConnectedItems(originalImage, edges, j, i - 1, threshold);
                            }
                        }
                    }

                    (new AdaptiveSmooth()).Apply(edges);// smooth image inplace

                    for (int i = 0; i < originalImage.Height; i++)
                    {
                        for (int j = 1; j < originalImage.Width; j++)
                        {
                            if (edges.GetPixel(j, i).ToArgb() != black.ToArgb())
                            {
                                retVal.SetPixel(j, i, originalImage.GetPixel(j, i));
                            }
                        }
                    }

                    return retVal;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the connected items.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="edges">The edges.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="threshold">The threshold.</param>
        private static void getConnectedItems(Bitmap image, Bitmap edges, int x, int y, int threshold)
        {
            if (x < image.Width && y < image.Height)
            {
                Color white = Color.White;
                //x+1,y
                if ((image.Width > x + 1) && (image.GetPixel(x + 1, y).ToArgb() - image.GetPixel(x, y).ToArgb() < threshold))
                {
                    edges.SetPixel(x + 1, y, white);
                }
                //x+1,y+1
                if ((image.Width > x + 1 && image.Height > y + 1) && (image.GetPixel(x + 1, y + 1).ToArgb() - image.GetPixel(x, y).ToArgb() < threshold))
                {
                    edges.SetPixel(x + 1, y + 1, white);
                }
                //x+1,y-1
                if ((image.Width > x + 1 && y - 1 >= 0) && (image.GetPixel(x + 1, y - 1).ToArgb() - image.GetPixel(x, y).ToArgb() < threshold))
                {
                    edges.SetPixel(x + 1, y - 1, white);
                }
                //x-1,y
                if ((x - 1 >= 0 && x - 1 < image.Width) && (image.GetPixel(x - 1, y).ToArgb() - image.GetPixel(x, y).ToArgb() < threshold))
                {
                    edges.SetPixel(x - 1, y, white);
                }
                //x-1,y+1
                if ((x - 1 >= 0 && image.Height > y + 1) && (image.GetPixel(x - 1, y + 1).ToArgb() - image.GetPixel(x, y).ToArgb() < threshold))
                {
                    edges.SetPixel(x - 1, y + 1, white);
                }
                //x-1,y-1
                if ((x - 1 >= 0 && y - 1 >= 0) && (image.GetPixel(x - 1, y - 1).ToArgb() - image.GetPixel(x, y).ToArgb() < threshold))
                {
                    edges.SetPixel(x - 1, y - 1, white);
                }
                //x,y+1
                if ((image.Height > y + 1) && (image.GetPixel(x, y + 1).ToArgb() - image.GetPixel(x, y).ToArgb() < threshold))
                {
                    edges.SetPixel(x, y + 1, white);
                }
                //x,y-1
                if ((y - 1 >= 0) && (image.GetPixel(x, y - 1).ToArgb() - image.GetPixel(x, y).ToArgb() < threshold))
                {
                    edges.SetPixel(x, y - 1, white);
                }
            }
        }

        /// <summary>
        /// Save the item to the checkerboard result file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkerboardMenu_Click(object sender, EventArgs e)
        {
            Bitmap tempImage = AForge.Imaging.Image.Clone(image);
            double[] values = new double[3];
            for (int i = 1; i <= 3; i++)
            {
                if (i == 2)
                {
                    MessageBox.Show("Please select a value arround 150",
                        "Signature Verification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (i == 3)
                {
                    MessageBox.Show("Please select a value arround 180.",
                    "Signature Verification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Cursor.Current = Cursors.WaitCursor;
                if (i > 1)
                {
                    thresholdingSegment_Click(new object(), new EventArgs());
                }
                ImageStatistics stat = (image == null) ? null : new ImageStatistics(image);

                Bitmap map = AForge.Imaging.Image.Clone(image);
                SusanCornersDetector cnrDetector = new SusanCornersDetector();
                cnrDetector.ProcessImage(AForge.Imaging.Filters.NewGrayscale.CommonAlgorithms.BT709.Apply(map));
                //cnrDetector.ProcessImage(map);
                List<AForgeService.IntPoint> corners = cnrDetector.ProcessImage(image);

                double temp = ((float)corners.Count * 100) / (float)stat.PixelsCountWithoutBlack;

                //get main form for work
                values[i - 1] = temp;

                if (i == 2)
                {
                    image = tempImage;
                }
            }

            List<string> resultList = new System.Collections.Generic.List<string>();
            resultList.Add(values[0].ToString());
            resultList.Add(values[1].ToString());
            resultList.Add(values[2].ToString());
            string result = getResultValue(values);
            resultList.Add(result);
            resultList.Add(getFilePath());
            writeToMetadataFile(resultList, "che"); //push results to the file

            //finally give the message
            MessageBox.Show("Percentage of analysed region could be a printed region is : " + (100 - Double.Parse(getResultValue(values))).ToString(),
                        "Signature Verification", MessageBoxButtons.OK, MessageBoxIcon.Information);

            /*remove after test
            try
            {
                string outFile = "C://Users//Tharindu//Desktop//Project//accuracy test//original.csv";

                using (StreamWriter writer = File.AppendText(outFile))
                {
                    writer.WriteLine(result);
                }

            }
            catch (Exception)
            {
                MessageBox.Show(this, "Fail to save the text", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            /*end remove after test*/
        }

        /// <summary>
        /// get file path from main form
        /// </summary>
        /// <returns></returns>
        private string getFilePath()
        {
            MainForm mainForm = this.TopLevelControl as MainForm;
            return mainForm.getCurrentFileNme();
        }


        /// <summary>
        /// get results to checkerboard
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string getResultValue(double[] input)
        {
            string retVal = string.Empty;
            double nonThresholdPercent;
            double Threshold150Percent;
            double Threshold180Percent;

            //for non threshold value
            if (input[0] > 5)
            {
                nonThresholdPercent = 5;
            }
            else if (input[1] > 2.5)
            {
                nonThresholdPercent = 5 + 20 * ((input[1] - 2.5) / 2.5);
            }
            else if (input[1] > 0.88)
            {
                nonThresholdPercent = 15 + 30 * ((input[1] - 0.88) / 1.62);
            }
            else if (input[1] > 0.6)
            {
                nonThresholdPercent = 45 + 35 * (input[0] - 0.5);
            }
            else
            {
                nonThresholdPercent = 80;
            }

            //for 150 threshold
            if (input[1] > 0 && input[1] < 3.5)
            {
                Threshold150Percent = 40 * ((input[1] - 0) / 4);
            }
            else if (input[1] < 7)
            {
                Threshold150Percent = 40 + 40 * ((input[1] - 3.5) / 3.5);
            }
            else
            {
                Threshold150Percent = 80;
            }

            //for 180 threshold
            if (input[2] < 4.3)
            {
                Threshold180Percent = 10;
            }
            else if (input[2] < 6.16)
            {
                Threshold180Percent = 10 + 70 * ((input[2] - 4.3) / 1.86);
            }
            else
            {
                Threshold180Percent = 80;
            }

            //combine generated result
            double result = (.4 * nonThresholdPercent + .3 * Threshold150Percent + .3 * Threshold180Percent) + 5;
            if (result > 100) { result = 100; }
            retVal = (100 - result).ToString();
            return retVal;
        }


        #endregion

        #region Dithering

        public void UniformCropDone()
        {
            
            cropCount += 1;
            if (cropCount == 1)
            {
                colorArray1 = GetColorVariation();
                //WriteColorVarianceResearch(colorArray1);
                this.image.Dispose();
                this.image = (Bitmap)ditheringImage.Clone();
                UpdateNewImage();

                MessageBox.Show("Please select another uniform color region in the signature to verify.",
                    "Signature Verification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                UniformCrop();
            }

            else if (cropCount == 2)
            {
                colorArray2 = GetColorVariation();
                //WriteColorVarianceResearch(colorArray2);
                this.image.Dispose();
                this.image = (Bitmap)ditheringImage.Clone();
                UpdateNewImage();

                MessageBox.Show("Please select another uniform color region in the signature to verify.",
                    "Signature Verification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                UniformCrop();
            }

            else if (cropCount == 3)
            {
                colorArray3 = GetColorVariation();

                WriteColorVarianceResearch(colorArray1);
                WriteColorVarianceResearch(colorArray2);
                WriteColorVarianceResearch(colorArray3);

                //// Create an instance of the dialog
                //PenColor input = new PenColor();
                //// Show the dialog modally, testing the result.
                //// If the user cancelled, skip past this block.
                //if (input.ShowDialog() == DialogResult.OK)
                //{
                //    // The user clicked OK or pressed Return Key
                //    // so display their input in this form.
                //    //get text like this  "input.comboBox1.SelectedItem.ToString();"
                //}


                //BluePenCalc(colorArray1[0] + colorArray2[0] + colorArray3[0], colorArray1[1] + colorArray2[1] + colorArray3[1], colorArray1[2] + colorArray2[2] + colorArray3[2]);
                int redColorVar = colorArray1[0] + colorArray2[0] + colorArray3[0];
                int greenColorVar = colorArray1[1] + colorArray2[1] + colorArray3[1];
                int blueColorVar = colorArray1[2] + colorArray2[2] + colorArray3[2];


                string[] dithPercentage = new string[2];
                dithPercentage = BluePenCalc(redColorVar, greenColorVar, blueColorVar);

                string dithering1 = "Blue Color Pen";
                string dithering2 = "Red Color Variacne: " + redColorVar.ToString() + ", Green Color Variacne: " + greenColorVar + ", Blue Color Variacne: " + blueColorVar;

                WriteDitheringData(dithering1, dithering2, dithPercentage[0], dithPercentage[1]);

                this.image.Dispose();
                this.image = (Bitmap)ditheringImage.Clone();
                UpdateNewImage();

                MessageBox.Show("Color variation report generated.",
                    "Signature Verification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cropCount = 0;

                MainForm mainForm = this.TopLevelControl as MainForm;
                mainForm.visibleFalseToolbars();
                ditheringImage.Dispose();
            }
        }


        public void WriteColorVariance(int[] pixelArray)
        {
            string output = "";

            output += "Red Color Variance : " + pixelArray[0].ToString() + "; ";
            output += "Green Color Variance : " + pixelArray[1].ToString() + "; ";
            output += "Blue Color Variance : " + pixelArray[2].ToString() + ";";

            MainForm mainForm = this.TopLevelControl as MainForm;
            mainForm.saveDitheringTxt(output);
        }

        public void WriteColorVarianceResearch(int[] pixelArray)
        {
            try
            {
                string fileName = "A:\\Red.txt";
                using (System.IO.StreamWriter txtWriter = new System.IO.StreamWriter(@fileName, true))
                {
                    txtWriter.WriteLine(pixelArray[0].ToString());
                }

                fileName = "A:\\Green.txt";
                using (System.IO.StreamWriter txtWriter = new System.IO.StreamWriter(@fileName, true))
                {
                    txtWriter.WriteLine(pixelArray[1].ToString());
                }

                fileName = "A:\\Blue.txt";
                using (System.IO.StreamWriter txtWriter = new System.IO.StreamWriter(@fileName, true))
                {
                    txtWriter.WriteLine(pixelArray[2].ToString());
                }

            }
            catch (Exception)
            {
                MessageBox.Show(this, "Fail to save the text", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Write dithering verification results to the final report
        /// </summary>
        /// <param name="dithering1"></param>
        /// <param name="dithering2"></param>
        /// <param name="dithering3"></param>
        /// <param name="dithering4"></param>
        public void WriteDitheringData(string dithering1,string dithering2,string dithering3,string dithering4) 
        {
            List<string> resultList = new System.Collections.Generic.List<string>();
            resultList.Add(dithering1);
            resultList.Add(dithering2);
            resultList.Add(dithering3);
            resultList.Add(dithering4);
            writeToMetadataFile(resultList, "dit");
        }

        /// <summary>
        /// Calculate percentage for blue pens
        /// </summary>
        /// <param name="totalRedVar"> Red color variation </param>
        /// <param name="totalGreenVar"> Green color variation </param>
        /// <param name="totalBlueVar"> Blue color variation </param>
        public string[] BluePenCalc(int totalRedVar, int totalGreenVar, int totalBlueVar)
        {
            double redeVar = totalRedVar / 3;
            double redPercentage = 100;

            if (redeVar < 76.23)
            {
                redPercentage = 8.33;
            }
            else if (redeVar >= 121.08)
            {
                redPercentage = 95;
            }
            else 
            {
                redPercentage = 8.33 + 86.67 * (redeVar - 76.23) / 44.85;
            }

            double greenVar = totalBlueVar / 3;
            double greenPercentage = 100;

            if (greenVar < 50.18)
            {
                greenPercentage = 6.67;
            }
            else if (greenVar >= 97.09)
            {
                greenPercentage = 98.33;
            }
            else
            {
                greenPercentage = 6.67 + 88.33 * (greenVar - 50.18) / 46.91;
            }

            double blueVar = totalBlueVar / 3;
            double bluePercentage = 100;

            if (blueVar < 61.72)
            {
                bluePercentage = 5;
            }
            else if (blueVar > 95.6)
            {
                bluePercentage = 93.33;
            }
            else
            {
                bluePercentage = 5 + 90 * (blueVar - 61.72) / 33.88;
            }

            double totalFalsePercentage = Math.Max(redPercentage,greenPercentage);
            totalFalsePercentage = Math.Max(totalFalsePercentage, bluePercentage);

            string[] ditheringPercentage = new string[2];
            ditheringPercentage[0] = "Suspect Percentages - Red Color: " + redPercentage.ToString() + ", Green Color: " + greenPercentage.ToString() + ", Blue Color: " + bluePercentage.ToString() ;
            ditheringPercentage[1] = totalFalsePercentage.ToString();
            WritePercentage(redPercentage, greenPercentage, bluePercentage, totalFalsePercentage);

            return ditheringPercentage;
        }

        /// <summary>
        /// Calculate percentage for black color pens
        /// </summary>
        /// <param name="totalRedVar"> Red color variation </param>
        /// <param name="totalGreenVar"> Green color variation </param>
        /// <param name="totalBlueVar"> Blue color variation </param>
        public void BlackPenCalc(int totalRedVar, int totalGreenVar, int totalBlueVar)
        {
            double redeVar = totalRedVar / 3;
            double redPercentage = 100;

            if (redeVar < 132.9)
            {
                redPercentage = 2.38;
            }
            else if (redeVar >= 209.47)
            {
                redPercentage = 95;
            }
            else
            {
                redPercentage = 2.38 + 92.62 * (redeVar - 132.9) / 76.57;
            }

            double greenVar = totalBlueVar / 3;
            double greenPercentage = 100;

            if (greenVar < 133.9)
            {
                greenPercentage = 2.38;
            }
            else if (greenVar >= 208.4)
            {
                greenPercentage = 95;
            }
            else
            {
                greenPercentage = 2.38 + 92.62 * (greenVar - 133.9) / 74.5;
            }

            double blueVar = totalBlueVar / 3;
            double bluePercentage = 100;

            if (blueVar < 132.6)
            {
                bluePercentage = 2.38;
            }
            else if (blueVar > 207.45)
            {
                bluePercentage = 95;
            }
            else
            {
                bluePercentage = 2.38 + 92.62 * (blueVar - 132.6) / 74.85;
            }

            double totalFalsePercentage = Math.Max(redPercentage, greenPercentage);
            totalFalsePercentage = Math.Max(totalFalsePercentage, bluePercentage);

            WritePercentage(redPercentage, greenPercentage, bluePercentage, totalFalsePercentage);
        }


        public void WritePercentage(double red, double green, double blue, double total)
        {
            try
            {
                string fileName = "A:\\Percentage.txt";
                using (System.IO.StreamWriter txtWriter = new System.IO.StreamWriter(@fileName, true))
                {
                    txtWriter.WriteLine(total);
                }

            }
            catch (Exception)
            {
                MessageBox.Show(this, "Fail to save the text", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UniformCropCancel()
        {
            cropCount = 0;
            this.image = ditheringImage;
            MainForm mainForm = this.TopLevelControl as MainForm;
            mainForm.visibleFalseToolbars();
            imageItem.Enabled = true;
            filtersItem.Enabled = true;
        }

        public int[] GetColorVariation()
        {
            Bitmap temp = this.image;
            Bitmap bmap = (Bitmap)temp.Clone();
            Color c;
            int[] colorVarianceArray = new int[3];

            int redMin = 255;
            int redMax = 0;
            int greenMin = 255;
            int greenMax = 0;
            int blueMin = 255;
            int blueMax = 0;

            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);

                    if (c.R < redMin)
                    {
                        redMin = c.R;
                    }

                    if (c.R > redMax)
                    {
                        redMax = c.R;
                    }

                    if (c.B < blueMin)
                    {
                        blueMin = c.B;
                    }

                    if (c.B > blueMax)
                    {
                        blueMax = c.B;
                    }

                    if (c.G < greenMin)
                    {
                        greenMin = c.G;
                    }

                    if (c.G > greenMax)
                    {
                        greenMax = c.G;
                    }
                }
            }

            colorVarianceArray[0] = redMax - redMin;
            colorVarianceArray[1] = greenMax - greenMin;
            colorVarianceArray[2] = blueMax - blueMin;

            return colorVarianceArray;
        }

        #endregion


        private void menuItem4_Click(object sender, EventArgs e)
        {
            getVariationofDepth();
        }

        private void menuItem9_Click(object sender, EventArgs e)
        {
        }

        private void ImageDoc_MouseClick(object sender, MouseEventArgs e)
        {
            //Point imagePoint, nothing;
            //GetImageAndScreenPoints(new Point(e.X, e.Y), out imagePoint, out nothing);
            //MessageBox.Show(imagePoint.ToString() + "       " + LineStart.ToString() + "       " + lineLength.ToString());
        }


        

        
        private void ditheringMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please select a uniform color region in the signature to verify.",
                    "Signature Verification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            imageItem.Enabled = false;
            filtersItem.Enabled = false;
            ditheringImage = (Bitmap)this.image.Clone();
            UniformCrop();
        }

        


    }

    // Selection arguments
    public class SelectionEventArgs : EventArgs
    {
        private Point location;
        private Size size;

        // Constructors
        public SelectionEventArgs(Point location)
        {
            this.location = location;
        }
        public SelectionEventArgs(Point location, Size size)
        {
            this.location = location;
            this.size = size;
        }

        // Location property
        public Point Location
        {
            get { return location; }
        }
        // Size property
        public Size Size
        {
            get { return size; }
        }
    }

    // Commands
    public enum ImageDocCommands
    {
        Clone,
        Crop,
        ZoomIn,
        ZoomOut,
        ZoomOriginal,
        FitToSize,
        Levels,
        Grayscale,
        Threshold,
        Morphology,
        Convolution,
        Resize,
        Rotate,
        Brightness,
        Contrast,
        Saturation,
        Fourier
    }
}
