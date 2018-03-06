using System;

namespace DShop.Services.Operations.Dtos
{
    public class OperationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Resource { get; set; }
        public string Message { get; set; }
        public string State { get; set; }
        public bool Completed { get; set; }
        public bool Success { get; set; }        
    }
}