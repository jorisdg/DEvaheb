using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEvahebLib.Enums;

namespace DEvahebLib.Nodes
{
    public class Camera : FunctionNode
    {
        public Camera(params Node[] args)
            : base("camera", args)
        {
        }

        public CAMERA_COMMANDS? GetCommand()
        {
            if (arguments.Count > 0 && arguments[0] is EnumFloatValue enumValue && enumValue.Name == nameof(CAMERA_COMMANDS))
                return (CAMERA_COMMANDS)(float)enumValue.Value;

            return null;
        }

        public override int ExpectedArgCount
        {
            get
            {
                var cmd = GetCommand();
                if (cmd == null) return 1;

                switch (cmd.Value)
                {
                    case CAMERA_COMMANDS.DISABLE:
                    case CAMERA_COMMANDS.ENABLE:
                        return 1;
                    case CAMERA_COMMANDS.PATH:
                        return 2;
                    case CAMERA_COMMANDS.ZOOM:
                    case CAMERA_COMMANDS.MOVE:
                    case CAMERA_COMMANDS.ROLL:
                    case CAMERA_COMMANDS.DISTANCE:
                    case CAMERA_COMMANDS.SHAKE:
                        return 3;
                    case CAMERA_COMMANDS.PAN:
                    case CAMERA_COMMANDS.TRACK:
                    case CAMERA_COMMANDS.FOLLOW:
                        return 4;
                    case CAMERA_COMMANDS.FADE:
                        return 6;
                    default:
                        return 1;
                }
            }
        }

        public override List<Type[]> ExpectedArgTypes => new()
        {
            new[] { typeof(EnumIdentifierValue) }, // ENABLE, DISABLE
            new[] { typeof(EnumIdentifierValue), typeof(StringValue) }, // PATH
            new[] { typeof(EnumIdentifierValue), typeof(FloatValue), typeof(FloatValue) }, // ZOOM, DISTANCE, ROLL, SHAKE
            new[] { typeof(EnumIdentifierValue), typeof(VectorValue), typeof(FloatValue) }, // MOVE
            new[] { typeof(EnumIdentifierValue), typeof(VectorValue), typeof(VectorValue), typeof(FloatValue) }, // PAN
            new[] { typeof(EnumIdentifierValue), typeof(StringValue), typeof(FloatValue), typeof(FloatValue) }, // TRACK, FOLLOW
            new[] { typeof(EnumIdentifierValue), typeof(VectorValue), typeof(FloatValue), typeof(VectorValue), typeof(FloatValue), typeof(FloatValue) }, // FADE
        };
    }
}
