﻿namespace VisualPlus.Controls
{
    #region Namespace

    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Windows.Forms;

    using VisualPlus.Framework;
    using VisualPlus.Framework.GDI;
    using VisualPlus.Localization;

    #endregion

    /// <summary>The visual ToolTip.</summary>
    [ToolboxBitmap(typeof(ToolTip))]
    [Designer(VSDesignerBinding.VisualToolTip)]
    public sealed class VisualToolTip : ToolTip
    {
        #region Variables

        private Color[] backgroundColor =
            {
                ControlPaint.Light(Settings.DefaultValue.Style.BackgroundColor(0)),
                Settings.DefaultValue.Style.BackgroundColor(0),
                ControlPaint.Light(Settings.DefaultValue.Style.BackgroundColor(0))
            };

        private Color borderColor = Settings.DefaultValue.Style.BorderColor(0);
        private int borderThickness = Settings.DefaultValue.BorderThickness;
        private bool borderVisible = Settings.DefaultValue.BorderVisible;
        private Point endPoint;
        private Font font = new Font(Settings.DefaultValue.Style.FontFamily, 8.25F, FontStyle.Regular);
        private Color foreColor = Settings.DefaultValue.Style.ForeColor(0);
        private float gradientAngle;
        private LinearGradientBrush gradientBrush;
        private float[] gradientPosition = { 0, 1 / 2f, 1 };
        private Image icon;
        private bool iconBorder;
        private GraphicsPath iconGraphicsPath;
        private Point iconPoint = new Point(0, 0);
        private Rectangle iconRectangle;
        private Size iconSize = new Size(24, 24);
        private Color lineColor = Settings.DefaultValue.Style.BorderColor(0);
        private Padding padding;
        private Rectangle separator;
        private int separatorThickness = 1;
        private int spacing = 2;
        private Point startPoint;
        private string text;
        private Point textPoint;
        private TextRenderingHint textRendererHint = Settings.DefaultValue.TextRenderingHint;
        private bool textShadow;
        private string title;
        private Color titleColor = Color.Gray;
        private Font titleFont = new Font(Settings.DefaultValue.Style.FontFamily, 8.25F, FontStyle.Bold);
        private Point titlePoint;
        private ToolTipType toolTipType = ToolTipType.Default;

        #endregion

        #region Constructors

        public VisualToolTip()
        {
            Padding = new Padding(4, 4, 4, 4);
            IsBalloon = false;
            OwnerDraw = true;
            Popup += VisualToolTip_Popup;
            Draw += VisualToolTip_Draw;
        }

        public enum ToolTipType
        {
            /// <summary>The default.</summary>
            Default = 0,

            /// <summary>The image.</summary>
            Image = 1,

            /// <summary>The text.</summary>
            Text = 2
        }

        #endregion

        #region Properties

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.ComponentColor)]
        public Color[] BackgroundColor
        {
            get
            {
                return backgroundColor;
            }

            set
            {
                backgroundColor = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.BorderColor)]
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }

            set
            {
                borderColor = value;
            }
        }

        [DefaultValue(Settings.DefaultValue.BorderThickness)]
        [Category(Localize.Category.Layout)]
        [Description(Localize.Description.BorderThickness)]
        public int BorderThickness
        {
            get
            {
                return borderThickness;
            }

            set
            {
                if (ExceptionHandler.ArgumentOutOfRangeException(value, Settings.MinimumBorderSize, Settings.MaximumBorderSize))
                {
                    borderThickness = value;
                }
            }
        }

        [DefaultValue(Settings.DefaultValue.BorderVisible)]
        [Category(Localize.Category.Behavior)]
        [Description(Localize.Description.BorderVisible)]
        public bool BorderVisible
        {
            get
            {
                return borderVisible;
            }

            set
            {
                borderVisible = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.ComponentFont)]
        public Font Font
        {
            get
            {
                return font;
            }

            set
            {
                font = value;
            }
        }

        [Category(Localize.Category.Behavior)]
        [Description(Localize.Description.Angle)]
        public float GradientAngle
        {
            get
            {
                return gradientAngle;
            }

            set
            {
                gradientAngle = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.GradientPosition)]
        public float[] GradientPosition
        {
            get
            {
                return gradientPosition;
            }

            set
            {
                gradientPosition = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.Icon)]
        public Image Icon
        {
            get
            {
                return icon;
            }

            set
            {
                icon = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.BorderVisible)]
        public bool IconBorder
        {
            get
            {
                return iconBorder;
            }

            set
            {
                iconBorder = value;
            }
        }

        [Category(Localize.Category.Layout)]
        [Description(Localize.Description.IconSize)]
        public Size IconSize
        {
            get
            {
                return iconSize;
            }

            set
            {
                iconSize = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.ComponentColor)]
        public Color Line
        {
            get
            {
                return lineColor;
            }

            set
            {
                lineColor = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.ComponentNoName)]
        public Padding Padding
        {
            get
            {
                return padding;
            }

            set
            {
                padding = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.ComponentNoName)]
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.TextColor)]
        public Color TextColor
        {
            get
            {
                return foreColor;
            }

            set
            {
                foreColor = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.TextRenderingHint)]
        public TextRenderingHint TextRendering
        {
            get
            {
                return textRendererHint;
            }

            set
            {
                textRendererHint = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.ComponentVisible)]
        public bool TextShadow
        {
            get
            {
                return textShadow;
            }

            set
            {
                textShadow = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.ComponentNoName)]
        public ToolTipType TipType
        {
            get
            {
                return toolTipType;
            }

            set
            {
                toolTipType = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.ComponentNoName)]
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.TextColor)]
        public Color TitleColor
        {
            get
            {
                return titleColor;
            }

            set
            {
                titleColor = value;
            }
        }

        [Category(Localize.Category.Appearance)]
        [Description(Localize.Description.ComponentFont)]
        public Font TitleFont
        {
            get
            {
                return titleFont;
            }

            set
            {
                titleFont = value;
            }
        }

        #endregion

        #region Events

        /// <summary>Input the text height to compare it to the icon height.</summary>
        /// <param name="textHeight">The text height.</param>
        /// <returns>New height.</returns>
        private int GetTipHeight(int textHeight)
        {
            int tipHeight = textHeight > iconSize.Height ? textHeight : iconSize.Height;
            return tipHeight;
        }

        /// <summary>Input the title and text width to retrieve total width.</summary>
        /// <param name="titleWidth">The title width.</param>
        /// <param name="textWidth">The text width.</param>
        /// <returns>New width.</returns>
        private int GetTipWidth(int titleWidth, int textWidth)
        {
            int tipWidth = titleWidth > iconSize.Width + textWidth ? titleWidth : iconSize.Width + textWidth;
            return tipWidth;
        }

        private void VisualToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = textRendererHint;

            startPoint = new Point(e.Bounds.Width, 0);
            endPoint = new Point(e.Bounds.Width, e.Bounds.Height);

            gradientBrush = GDI.CreateGradientBrush(backgroundColor, gradientPosition, gradientAngle, startPoint, endPoint);

            // Background
            graphics.FillRectangle(gradientBrush, e.Bounds);

            // Create border
            if (borderVisible)
            {
                Rectangle border = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
                GraphicsPath borderPath = new GraphicsPath();
                borderPath.AddRectangle(border);

                // Draw border
                graphics.DrawPath(new Pen(borderColor, borderThickness), borderPath);
            }

            // Draw the text shadow
            if (textShadow && toolTipType == ToolTipType.Text || textShadow && toolTipType == ToolTipType.Default)
            {
                graphics.DrawString(text, new Font(Font, FontStyle.Regular), Brushes.Silver, new PointF(textPoint.X + 1, textPoint.Y + 1));
            }

            switch (toolTipType)
            {
                case ToolTipType.Default:
                    {
                        // Draw the title
                        graphics.DrawString(title, titleFont, new SolidBrush(titleColor), new PointF(titlePoint.X, titlePoint.Y));

                        // Draw the separator
                        graphics.DrawLine(new Pen(lineColor), separator.X, separator.Y, separator.Width, separator.Y);

                        // Draw the text
                        graphics.DrawString(text, Font, new SolidBrush(foreColor), new PointF(textPoint.X, textPoint.Y));

                        if (Icon != null)
                        {
                            // Update point
                            iconRectangle.Location = iconPoint;

                            // Draw icon border
                            if (iconBorder)
                            {
                                graphics.DrawPath(new Pen(borderColor), iconGraphicsPath);
                            }

                            // Draw icon
                            graphics.DrawImage(Icon, iconRectangle);
                        }

                        break;
                    }

                case ToolTipType.Image:
                    {
                        if (Icon != null)
                        {
                            // Update point
                            iconRectangle.Location = iconPoint;

                            // Draw icon border
                            if (iconBorder)
                            {
                                graphics.DrawPath(new Pen(borderColor), iconGraphicsPath);
                            }

                            // Draw icon
                            graphics.DrawImage(Icon, iconRectangle);
                        }

                        break;
                    }

                case ToolTipType.Text:
                    {
                        // Draw the text
                        graphics.DrawString(text, Font, new SolidBrush(foreColor), new PointF(textPoint.X, textPoint.Y));
                        break;
                    }
            }

            gradientBrush.Dispose();
        }

        private void VisualToolTip_Popup(object sender, PopupEventArgs e)
        {
            var xWidth = 0;
            var yHeight = 0;

            switch (toolTipType)
            {
                case ToolTipType.Default:
                    {
                        xWidth = GetTipWidth(TextRenderer.MeasureText(title, Font).Width, TextRenderer.MeasureText(text, Font).Width);
                        yHeight = TextRenderer.MeasureText(title, Font).Height + separatorThickness + GetTipHeight(TextRenderer.MeasureText(text, Font).Height);

                        titlePoint.X = padding.Left;
                        titlePoint.Y = padding.Top;

                        Point separatorPoint = new Point(padding.Left + spacing, TextRenderer.MeasureText(title, Font).Height + 5);
                        Size separatorSize = new Size(xWidth - spacing, separatorThickness);
                        separator = new Rectangle(separatorPoint, separatorSize);

                        textPoint.X = padding.Left + iconSize.Width + spacing;
                        textPoint.Y = separator.Y + spacing;

                        iconPoint = new Point(padding.Left, textPoint.Y);
                        break;
                    }

                case ToolTipType.Image:
                    {
                        iconPoint = new Point(padding.Left, padding.Top);
                        xWidth = iconSize.Width + 1;
                        yHeight = iconSize.Height + 1;
                        break;
                    }

                case ToolTipType.Text:
                    {
                        textPoint = new Point(padding.Left, padding.Top);
                        xWidth = TextRenderer.MeasureText(text, Font).Width;
                        yHeight = TextRenderer.MeasureText(text, Font).Height;
                        break;
                    }
            }

            // Create icon rectangle
            iconRectangle = new Rectangle(iconPoint, iconSize);

            // Create icon path
            iconGraphicsPath = new GraphicsPath();
            iconGraphicsPath.AddRectangle(iconRectangle);
            iconGraphicsPath.CloseAllFigures();

            // Initialize new size
            e.ToolTipSize = new Size(padding.Left + xWidth + padding.Right, padding.Top + yHeight + padding.Bottom);
        }

        #endregion
    }
}