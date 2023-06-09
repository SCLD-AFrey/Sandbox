﻿using DevExpress.Xpo;

namespace VikingEnterprise.Server.Services.Host.Database;

public interface IDatabaseInterface
{
    public IDataLayer DataLayer { get; }

    public UnitOfWork ProvisionUnitOfWork();
}