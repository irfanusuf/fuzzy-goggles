using System;
using MongoDB.Bson;

namespace WebApplication1.Models;

public class Product

{


      public ObjectId Id { get; set;}
      public required string Name  { get; set;}
      public required string Size { get; set;}
      public int Qty { get; set;}
      public required string Color { get; set;}



}
