namespace BeCool.Application.Infrastructure
{
    public abstract class PaginateModel
    {
        int pageIndex;
        int pageSize;
        public int PageIndex
        {
            get
            {
                return pageIndex < 1 ? 1 : pageIndex;
            }
            set
            {
                if (value >= 1)
                {
                    pageIndex = value;
                }
            }
        }
        public virtual int PageSize
        {
            get
            {
                return pageSize < 9 ? 9 : pageSize;
            }
            set
            {
                if (value >= 9)
                {
                    pageSize = value;
                }
            }
        }
    }
}
