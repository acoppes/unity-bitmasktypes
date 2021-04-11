# unity-bitmasktypes

# Introduction

The idea with this project is having a way of creating custom enums in editor, by Game Designers for example, and use them internally for fast comparisons as bitmasks.


Right now, each time a new type is created, the asset importer auto assigns a bitmask to that asset. The bitmask itself isn't important in editor, could change and nothing happens since the users of that asset should referencing the asset itself. 

To use it, you have to create a subclass of BaseTypeAsset and start creating your asset instances for each kind of type you need. For example, for a game with different kind of damages:

```
[CreateAssetMenu(menuName="Gemserk/Example/Damage Type")]
public class DamageTypeAsset : BaseTypeAsset {}
```

Then, in a class you want a bitmask of that type, you can do the following:

```
public class UnitWithDamage : MonoBehaviour
{
    [TypeMask(typeof(DamageTypeAsset))]
    public BaseTypeMask movement;
}
```

Here you declare a field of type BaseTypeMask, and say the type you want is of type `DamageTypeAsset` so the custom property drawer will allow you select the bits you want using an enum mask (internally saves a list of asset references, not the bitmask).

In editor, it looks like this:


![An example of how it looks in the editor](images/example1.png?raw=true "Example1")

# Motivation

When working on Iron Marines, we used bitmasks to speed up units' abilities targeting logic. To do that, we were using c# enums and using them in Unity as flags, like this:

```
[Flags]
public enum TargetType
{
    Unit = 1 << 0,
    Structure = 1 << 1,
    Vehicle = 1 << 2
}
```

So we could configure in editor by selecting the flags we wanted, and then we just compared using bitwise operations to check if a target matches the ability targeting parameters. This works pretty well internally, but at some point I wanted to delegate to the Game Designer team the decision of which enums they wanted for the game and at the same time, hide the fact that internally we need them to be bitmasks. 

Not sure if this is the right approach, I am just exploring some ideas.

## TODO

* Better Readme (maybe some image to explain better)
* Better example
* Project structure to upload to OpenUPM or something like that.
* Autogenerate code with masks to use directly in code.