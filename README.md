# Unity3dMVP

An mvp framework for unity3d ui, based on ugui and c# reflection.
Code model class, write present action or event method, then select the action or event at linker component.
The linker component will auto display fields in model class.
Bind these fields to ugui component by draging binder into linker fields.
When the action method is called, the return model will auto update to ugui component through binders.
