namespace ShoppingCartAPI.Models
{
    public class GDCTEntityBase<TId> : IGDCTEntityBase<TId>
    {
        public virtual TId? Id { get; set; }
        public int Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }

        public bool IsTransient()
        {
            return Id == null || Id.Equals(default(TId));
        }

        public override bool Equals(object? obj)
        {
            if (obj is not GDCTEntityBase<TId> item)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return item == this;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                // Use a readonly copy of the Id to ensure immutability
                var idHashCode = Id!.GetHashCode();
                return idHashCode ^ 31;
            }
            else
                return 0;
        }
    }
}
    
