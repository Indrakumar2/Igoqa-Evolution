using System;

[AttributeUsage(AttributeTargets.All)]
public class AuditNameAttribute : Attribute
{
    private readonly string _name;
    private readonly string _format;
    private readonly AuditNameformatDataType _formatDataType;

    public AuditNameAttribute(string auditDisplayName)
    {
        this._name = auditDisplayName;
    }

    public AuditNameAttribute(string auditDisplayName,string format)
    {
        this._name = auditDisplayName;
        this._format=format;
    }

    public AuditNameAttribute(string auditDisplayName, string format, AuditNameformatDataType formatDataType)
    {
        this._name = auditDisplayName;
        this._format = format;
        this._formatDataType = formatDataType;
    }

    public virtual string AuditName
    {
        get {return _name;}
    }

     public virtual string Format
    {
        get {return _format;}
    }

    public virtual AuditNameformatDataType FormatDataType
    {
        get { return _formatDataType; }
    }
}

public enum AuditNameformatDataType
{
    None,
    DateTime
}
