﻿namespace RanceConnect;

public class Provision
{
    string id;
    string ean;
    int quantity;

    DateTime expirationDate;
    DateTime dateAdded;


    public string ID { get => id; set => id = value; }
    public string EAN { get => ean; set => ean = value; }
    public int Quantity { get => quantity; set => quantity = value; }
    public DateTime ExpirationDate { get => expirationDate; set => expirationDate = value; }
    public DateTime DateAdded { get => dateAdded; set => dateAdded = value; }

    public Provision(string id, string ean, int quantity, DateTime expirationDate, DateTime dateAdded)
    {
        ID = id;
        EAN = ean;
        Quantity = quantity;
        ExpirationDate = expirationDate;
        DateAdded = dateAdded;
    }
}
