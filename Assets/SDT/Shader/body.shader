Shader "Unlit/body"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Zwrite On
            ColorMask 0

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }



        }
    }
}
