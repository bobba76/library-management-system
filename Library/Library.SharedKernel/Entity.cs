﻿namespace Library.SharedKernel;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}