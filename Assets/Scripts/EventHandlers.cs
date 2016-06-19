using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public interface ICellMouseEnterExitHandler : IEventSystemHandler
{
    void OnCellMouseEnter(Cell cell);
    void OnCellMouseExit(Cell cell);
}

public interface ICellMouseDownHandler : IEventSystemHandler
{
    void OnCellMouseDown(Cell cell);
}

public interface IUnitActionHandler : IEventSystemHandler
{
    void OnUnitAction(Unit unit, int damage, int pierce, Effect effect);
}

public interface IGameStateManagerHandler : IEventSystemHandler
{
    void OnUnitAction(Unit unit);
}