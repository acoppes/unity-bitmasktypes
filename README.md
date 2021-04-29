[![openupm](https://img.shields.io/npm/v/com.gemserk.bitmasktypes?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.gemserk.bitmasktypes/)

# Introduction

The idea with this project is having a way of creating custom enums in editor, by Game Designers for example, and use them internally for fast comparisons as bitmasks.

## Custom types

Right now, each time a new type is created, the asset importer auto assigns a bitmask to that asset. The bitmask itself isn't important in editor, could change and nothing happens since the users of that asset should referencing the asset itself. 

To use it, you have to create a subclass of BaseTypeAsset and start creating your asset instances for each kind of type you need. For example, for a game with different kind of damages:

```csharp
[CreateAssetMenu(menuName="Gemserk/Example/Damage Type")]
public class DamageTypeAsset : BaseTypeAsset {}
```

Then, in a class you want a bitmask of that type, you can do the following:

```csharp
public class UnitWithDamage : MonoBehaviour
{
    [TypeMask(typeof(DamageTypeAsset))]
    public BaseTypeMask damageTypes;
}
```

Here you declare a field of type BaseTypeMask, and say the type you want is of type `DamageTypeAsset` so the custom property drawer will allow you select the bits you want using an enum mask (internally saves a list of asset references, not the bitmask).

In editor, it looks like this:

![An example of how it looks in the editor](images/example1.png?raw=true "Example1")

### Code autogeneration

For each custom type, a file with an enum with bitmasks is autogenerated when reimporting new type instances, for example: 

```csharp
namespace Gemserk.Examples
{
    public enum DamageTypeAssetEnum
    {       
        ElectricDamage = 1 << 0,   
        FireDamage = 1 << 1,
        IceDamage = 1 << 2,
        PoisonDamage = 1 << 3,
    }
}

```

So this can be used from code directly. This normally isn't needed since we try to configure everything to depend on assets and then use an int, but sometimes this could be handy, maybe for unit tests, or for editor stuff.

## Custom type names

Another option is to just define your types in a generic way in your code and then use custom attribute to optionally override the names in the inspector. This is a clean an simplier way.

For example, for the following type:

```csharp
[Flags]
public enum ArmorType
{
    Nothing = 0,
    Everything = -1,
    Type0 = 1 << 0,
    Type1 = 1 << 1,
    Type2 = 1 << 2,
    Type3 = 1 << 3,
    Type4 = 1 << 4,
    Type5 = 1 << 5,
    Type6 = 1 << 6,
    Type7 = 1 << 7
}
```

The inspector will normally draw it as:

![Basic example](images/example_using_names4.png?raw=true "Example1")

In this case, you can create an `EnumNameTypeAsset` asset to override the names, as it follow:

![Basic example](images/example_using_names1.png?raw=true "Example1")

And then, configure code attribute when using the type:

```csharp
    [EnumName("damages")]
    public ArmorType type1;
```

That will show this in the inspector:

![Basic example](images/example_using_names2.png?raw=true "Example1")

This works with other enums and even with int fields.

```csharp
    [EnumName("damages")]
    public int type3;
```

# Motivation

When working on Iron Marines, we used bitmasks to speed up units' abilities targeting logic. To do that, we were using c# enums and using them in Unity as flags, like this:

```csharp
[Flags]
public enum TargetType
{
    Unit = 1 << 0,
    Structure = 1 << 1,
    Vehicle = 1 << 2
}
```

So we could configure in editor by selecting the flags we wanted, and then we just compared using bitwise operations to check if a target matches the ability targeting parameters. This works pretty well internally, but at some point I wanted to delegate to the Game Designer team the decision of which enums they wanted for the game and at the same time, hide the fact that internally we need them to be bitmasks.

I wanted the base concepts to be part of the engine, like for example the `damage type`, but the values to be part of the game using the core engine, like having `ice damage type` it might be useful for one game, but for another it makes no sense, wanted a way to decouple the final values from the core engine.

Not sure if this is the right approach, I am just exploring some ideas.

Another idea could've just been to use ints in the core engine and enums in the game using the engine, but for don't know how to link the two in the unity inspector and show an enum mask popup when editing the int from the core engine without having to add game related metadata in the core engine itself.