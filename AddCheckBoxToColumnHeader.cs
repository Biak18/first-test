using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System.Data;

namespace GV
{
    public class AddCheckEditToColumnHeader
    {
        private CheckEdit? checkEdit;
        private GridView? gridView;
        private GridColumn? gridColumn;
        private GridColumn[]? MulitipleColumnHeader;
        private int Width = 0;
        private int Height = 0;
        private Dictionary<GridColumn, CheckEdit> checkEdits = new Dictionary<GridColumn, CheckEdit>();

        /// <summary>
        /// Single column header
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="gridColumn"></param>
        public AddCheckEditToColumnHeader(GridView gridView, GridColumn gridColumn)
        {
            this.gridView = gridView;
            this.gridColumn = gridColumn;
            AddCheckEdit(false);
            InitializeEvent();
        }

        /// <summary>
        ///  Single column header
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="gridColumn"></param>
        public AddCheckEditToColumnHeader(GridView gridView, GridColumn gridColumn,int Width, int Heigh)
        {
            this.gridView = gridView;
            this.gridColumn = gridColumn;
            this.Width = Width;
            this.Height = Heigh;
            AddCheckEdit(false);
            InitializeEvent();
        }

        /// <summary>
        /// Multiple columns header
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="MulitipleColumnHeader"></param>
        /// <param name="Width"></param>
        /// <param name="Heigh"></param>
        public AddCheckEditToColumnHeader(GridView gridView, GridColumn[] MulitipleColumnHeader)
        {
            this.gridView = gridView;    
            this.MulitipleColumnHeader = MulitipleColumnHeader;
            AddCheckEdit(true);
            InitializeEvent();
        }

        /// <summary>
        /// Multiple columns header
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="MulitipleColumnHeader"></param>
        public AddCheckEditToColumnHeader(GridView gridView, GridColumn[] MulitipleColumnHeader,int Width, int Heigh)
        {
            this.gridView = gridView;
            this.Width = Width;
            this.Height = Heigh;
           this.MulitipleColumnHeader = MulitipleColumnHeader;
            AddCheckEdit(true);
            InitializeEvent();
        }

        private void InitializeEvent()
        {
            gridView!.CustomDrawColumnHeader += GridView_CustomDrawColumnHeader;
            gridView!.CellValueChanged += AddCheckEditToColumnHeader_CellValueChanged;
        }

        private void AddCheckEditToColumnHeader_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if(e.Column == gridColumn)
            {
                if(e.Column.ColumnType == typeof(bool))
                {

                }
            }
        }

        private void CheckEdit_CheckedChanged(object? sender, EventArgs e)
        {
            CheckEdit edit = (CheckEdit)sender!;
            if (MulitipleColumnHeader != null)
            {
                foreach (GridColumn column in gridView!.Columns)
                {
                    if (edit.Name == column.Name)
                    {
                        if(column.ColumnType == typeof(bool))
                        {
                            for (int i = 0; i < gridView.RowCount; i++)
                            {
                                gridView.SetRowCellValue(i, column, edit.CheckState);
                            }
                        }
                    }
                }
            }
            else if(gridColumn != null)
            {
                if (gridColumn.ColumnType == typeof(bool))
                {
                    for (int i = 0; i < gridView!.RowCount; i++)
                    {
                        gridView.SetRowCellValue(i, gridColumn, edit.CheckState);
                    }
                }
            }         
        }

        private void GridView_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (ColumnIsSelected(e.Column))
            {
                if(checkEdits.TryGetValue(e.Column, out var checkEdit))
                {
                    e.Info.InnerElements.Clear();
                    e.Painter.DrawObject(e.Info);
                    checkEdit!.Location = new Point(e.Bounds.Width - checkEdit.Width + e.Bounds.X, (e.Bounds.Height - checkEdit.Height) / 2 + e.Bounds.Y);
                    e.Handled = true;
                }
            }
        }

        private void AddCheckEdit(bool ismultiple)
        {
            if (ismultiple)
            {
                foreach (GridColumn col in MulitipleColumnHeader!)
                {
                    checkEdit = new CheckEdit();
                    checkEdit.Text = string.Empty;
                    checkEdit.Name = col.Name;
                    if (Width == 0)
                        Width = 19;
                    if (Height == 0)
                        Height = 19;

                    checkEdit.Width = Width;
                    checkEdit.Height = Height;
                    checkEdits.Add(col, checkEdit);
                    checkEdit.CheckStateChanged += CheckEdit_CheckedChanged;
                    gridView!.GridControl.Controls.Add(checkEdit);
                }
            }
            else
            {
                checkEdit = new CheckEdit();
                checkEdit.Text = string.Empty;

                if (Width == 0)
                    Width = 19;
                if (Height == 0)
                    Height = 19;

                checkEdit.Width = Width;
                checkEdit.Height = Height;
                checkEdits.Add(gridColumn!, checkEdit);
                checkEdit.CheckStateChanged += CheckEdit_CheckedChanged;
                gridView!.GridControl.Controls.Add(checkEdit);
            }       
        }

        private bool ColumnIsSelected(GridColumn column)
        {
            if (gridColumn != null && column == gridColumn)
            {
                return true;
            }
            else if (MulitipleColumnHeader != null)
            {
                if (gridColumn != null && Array.IndexOf(MulitipleColumnHeader, gridColumn) >= 0)
                {
                    return false;
                }
                else if (Array.IndexOf(MulitipleColumnHeader, column) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}