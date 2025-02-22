﻿using DevIO.NerdStore.Core.Messages;

namespace DevIO.NerdStore.Core.DomainObjects;

public abstract class Entity
{
    public Guid Id { get; set; }

    protected Entity() => Id = Guid.NewGuid();

    private readonly List<Event> _notificacoes = [];

    public IReadOnlyCollection<Event> Notificacoes => _notificacoes.AsReadOnly();

    public void AdicionarEvento(Event evento) => _notificacoes.Add(evento);

    public void RemoverEvento(Event evento) => _notificacoes.Remove(evento);

    public void LimparEventos() => _notificacoes.Clear();

    public override bool Equals(object? obj)
    {
        Entity? compareTo = obj as Entity;

        if (ReferenceEquals(this, compareTo))
            return true;

        if (compareTo is null)
            return false;

        return Id.Equals(compareTo.Id);
    }

    public override int GetHashCode() => GetType().GetHashCode() * 907 + Id.GetHashCode();

    public override string ToString() => $"{GetType().Name} [Id={Id}]";

    public static bool operator ==(Entity a, Entity b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b) => !(a == b);
}
