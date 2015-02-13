using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class Action  {

    /// <summary>
    /// Wraps the action in an ActionResult, allowing for alternate actions to be returned
    /// from an actions process method.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    static implicit operator ActionResult(Action action)
    {
        return new ActionResult(action);
    }

    /// <summary>
    /// Entity performing this aciton. It may me null.
    /// </summary>
    public Entity Entity { get { return _entity; } }

    /// <summary>
    /// Init a new instance of action
    /// </summary>
    /// <param name="entity">the entity performing this action</param>
    public Action(Entity entity)
    {
        _entity = entity;
    }

    /// <summary>
    /// Init a new instance of action.
    /// </summary>
    /// <param name="game"></param>
    public Action(Game game)
    {
        _game = game;
    }

    /// <summary>
    /// Mark this action as one that will consume energy when it is done successfully
    /// called by game when entity requests an action
    /// </summary>
    public void MarkForEnergyTaking()
    {
        _canConsumeEnergy = true;
    }

    public ActionResult Process(IList<Effect> effects, Queue<Action> actions)
    {
        _effects = effects;
        _acitons = actions;

        ActionResult result = OnProcess();

        _effects = null;
        _acitons = null;

        return result;
    }

    /// <summary>
    /// Adds an action to the current action queue
    /// </summary>
    /// <param name="action">New action to add</param>
    public void AddAction(Action action)
    {
        _acitons.Enqueue(action);
    }

    /// <summary>
    /// Adds a new effect
    /// </summary>
    /// <param name="effect">new effect</param>
    public void AddEffect(Effect effect)
    {
        _effects.Add(effect);
    }

    public void AfterSuccess()
    {
        if (_canConsumeEnergy && _entity != null)
        {
            //spend entities energy
            Debug.Log("Entity Spend Energy Not Implemented Action.cs");
            _canConsumeEnergy = false;
        }
    }

    public ActionResult Fail()
    {
        Debug.Log("Action Failed");
        return ActionResult.Fail;
    }

    protected abstract ActionResult OnProcess();

    //The entity performing this action.
    private IList<Effect> _effects;
    private Queue<Action> _acitons;
    private Entity _entity;
    private Game _game;
    private bool _canConsumeEnergy;
}
