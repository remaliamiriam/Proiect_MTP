# NavMesh Modifier Volume component reference

Use the NavMesh Modifier Volume component to change the area type of any [NavMeshes][1] within a defined region. The available properties allow you to define the affected region and specify the change in area type that you want. However, the modifier volume only affects the NavMeshes that are being newly built for the selected agent types. It has no effect on NavMeshes that already exist in the scene or in that volume and no effect on NavMeshes that get built for unaffected agent types.

You need to add the NavMesh Modifier Volume component to a [GameObject][2]. Though you can add the NavMesh Modifier Volume component to any GameObject in your scene, you typically add it to the GameObject that's associated with the NavMesh you want to affect.

To add the NavMesh Modifier Volume component to a GameObject, do the following:
1. Select the GameObject you want to use.
2. In the Inspector, select **Add Component** &gt; **Navigation** &gt; **NavMesh Modifier Volume**. <br/> The NavMesh Modifier Volume component is displayed in the Inspector window.

To change the area type of an entire GameObject, use the [NavMesh Modifier](./NavMeshModifier.md) component instead.

NavMesh Modifier Volume is useful when you need to assign an area type to part of your NavMesh that might not be represented as separate geometry. For example, you can use NavMesh Modifier Volume to make part of your NavMesh non-walkable or more difficult to cross.
The NavMesh Modifier Volume always assigns its area type when it overlaps with [NavMesh Modifier](./NavMeshModifier.md) objects, even if the area type of the volume has a lower index. When multiple volumes intersect, the area type with the highest index value out of all of them takes precedence. The exception to these rules is that the built-in Not Walkable area type assigned to any of the overlapping components is always the most important.

The NavMesh Modifier Volume affects the NavMesh generation process. As a result, the NavMesh is updated to reflect any changes to NavMesh Modifier Volumes.

The following table describes the properties available in the NavMesh Modifier Volume component.

| Property | Description |
| --- | --- |
| **Edit Volume** | Toggle the ability to edit the size of the volume in the Scene view. To modify the size of the volume as needed, select **Edit Volume**. A wire box with handles, representing the volume, is displayed in the Scene view. Drag the handles to modify the size of the volume. |
| **Size** | Specify the dimensions of the NavMesh Modifier Volume, defined by XYZ measurements. |
| **Center** | Specify the center of the NavMesh Modifier Volume relative to the center of the GameObject, defined by XYZ coordinates. |
| **Area Type** | Select the [area type](AreasAndCosts) that the NavMesh Modifier Volume applies to NavMeshes within the defined region. The available options include all of the area types that have a cost defined in the [Areas tab](NavigationWindow.html#areas-tab) of the Navigation window. |
| **Area Type** > **Open Area Settings** | Open the Areas tab of the Navigation window to define new area types or modify existing ones. |
| **Affected Agents** | Select the agent types for which the NavMesh Modifier Volume change applies. For example, you can make the selected NavMesh Modifier Volume a danger zone for specific agent types only. The available options include all of the agent types defined on the [Agents tab](NavigationWindow.html#areas-tab) of the Navigation window. |
| **Affected Agents** > **All** | Apply the change to all of the defined agent types whether now or in the future. |
| **Affected Agents** > **None** | Don't apply the change to any of the defined agent types. |
| **Affected Agents** > **Defined area types** | Apply the change to the selected agent types. You can select more than one agent type. |


## Additional resources
- [Create a NavMesh](./CreateNavMesh.md)
- [Navigation Areas and Costs](./AreasAndCosts.md)
- [Navigation Agent Types](./NavigationWindow.md#agents-tab)

[1]: ./Glossary.md#navmesh "A mesh that Unity generates to approximate the walkable areas and obstacles in your environment for path finding and AI-controlled navigation."

[2]: ./Glossary.md#gameobject "The fundamental object in Unity scenes, which can represent characters, props, scenery, cameras, waypoints, and more."

