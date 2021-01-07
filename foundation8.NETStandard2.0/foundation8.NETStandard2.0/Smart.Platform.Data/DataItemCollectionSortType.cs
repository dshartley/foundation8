namespace Smart.Platform.Data
{
    /// <summary>
    /// Encapsulates a type of sort for a data item collection
    /// </summary>
    public class DataItemCollectionSortType
    {
        private int     _index;
        private string  _name;
        private int     _sortPropertyEnum;
        private bool    _ascending = true;

        #region Constructors

        private DataItemCollectionSortType() { }

        public DataItemCollectionSortType(  int     index,
                                            string  name,
                                            int     sortPropertyEnum,
                                            bool    ascending)
        {
            _index              = index;
            _name               = name;
            _sortPropertyEnum   = sortPropertyEnum;
            _ascending          = ascending;
        }

        #endregion

        #region Public Methods

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int SortPropertyEnum
        {
            get { return _sortPropertyEnum; }
            set { _sortPropertyEnum = value; }
        }

        public bool Ascending
        {
            get { return _ascending; }
            set { _ascending = value; }
        }
        #endregion

    }
}
