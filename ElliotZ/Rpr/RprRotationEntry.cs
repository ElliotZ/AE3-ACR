using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using ElliotZ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElliotZ.Rpr;

public class RprRotationEntry : IRotationEntry, IDisposable
{
    public string AuthorName { get; set; } = Helper.AuthorName;
    private readonly Jobs _targetJob = Jobs.Reaper;
    private readonly AcrType _acrType = AcrType.Normal; 
    private readonly int _minLevel = 1;
    private readonly int _maxLevel = 100;
    private readonly string _description = "RPR试水";

    private List<SlotResolverData> SlotResolvers = new()
    {

    };

    public Rotation? Build(string settingFolder)
    {
        var rot = new Rotation(SlotResolvers)
        {
            TargetJob = _targetJob,
            AcrType = _acrType,
            MinLevel = _minLevel,
            MaxLevel = _maxLevel,
            Description = _description,
        };

        return rot;
    }

    public IRotationUI GetRotationUI() { return Qt.Instance; }
    public void OnDrawSetting() { }
    public void Dispose() { }
}
