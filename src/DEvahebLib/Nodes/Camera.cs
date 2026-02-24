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
                if (cmd == null) return -1;

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
                        return -1;
                }
            }
        }
    }

    public class Helper
    {
        public static bool ValidateArgumentEnumValue(Node argument, string enumName, float value)
        {
            return argument is EnumFloatValue enumValue && enumValue.Name == enumName && enumValue.Float == value;
        }

        public static bool ValidateArgumentEnumValue(Node argument, string enumName, int value)
        {
            return argument is EnumIntValue enumValue && enumValue.Name == enumName && enumValue.Integer == value;
        }

        public static bool ValidateArgumentEnumValue(Node argument, string enumName, string value)
        {
            return argument is EnumStringValue enumValue && enumValue.Name == enumName && enumValue.String == value;
        }

        public static bool ValidateArgumentValue(Node argument, float value)
        {
            return argument is FloatValue floatValue && floatValue.Float == value;
        }

        public static bool ValidateArgumentValue(Node argument, int value)
        {
            return argument is IntegerValue intValue && intValue.Integer == value;
        }

        public static bool ValidateArgumentValue(Node argument, string value)
        {
            return argument is StringValue stringValue && stringValue.String == value;
        }

        public static bool ValidateArgumentValue(Node argument, float x, float y, float z)
        {
            return argument is VectorValue vectorValue 
                && vectorValue.Values[0] is FloatValue xValue && xValue.Float == x
                && vectorValue.Values[1] is FloatValue yValue && yValue.Float == y
                && vectorValue.Values[2] is FloatValue zValue && zValue.Float == z;
        }
    }
}
