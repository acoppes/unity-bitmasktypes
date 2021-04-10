# unity-bitmasktypes

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

## TODO

* Better Readme
* Better examples
* Project structure to upload to OpenUPM or something like that.