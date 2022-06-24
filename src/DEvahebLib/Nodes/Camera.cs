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
        List<Node> arguments;

        public override IEnumerable<Node> Arguments => arguments;

        protected Camera(List<Node> arguments)
            : base(name: "camera")
        {
            this.arguments = arguments ?? new List<Node>();
        }

        public static Camera CreateCameraOverload(List<Node> arguments)
        {
            Camera newCamera = null;

            if (arguments.Count > 0 && arguments[0] is EnumFloatValue enumValue && enumValue.Name == nameof(CAMERA_COMMANDS))
            {
                switch((CAMERA_COMMANDS)(float)enumValue.Value)
                {
                    case CAMERA_COMMANDS.DISABLE:
                        newCamera = new CameraDisable(arguments);
                        break;
                    case CAMERA_COMMANDS.ENABLE:
                        newCamera = new CameraEnable(arguments);
                        break;
                    case CAMERA_COMMANDS.PAN:
                        newCamera = new CameraPan(arguments);
                        break;
                    case CAMERA_COMMANDS.ZOOM:
                        newCamera = new CameraZoom(arguments);
                        break;
                    case CAMERA_COMMANDS.MOVE:
                        newCamera = new CameraMove(arguments);
                        break;
                    case CAMERA_COMMANDS.ROLL:
                        newCamera = new CameraRoll(arguments);
                        break;
                    case CAMERA_COMMANDS.TRACK:
                        newCamera = new CameraTrack(arguments);
                        break;
                    case CAMERA_COMMANDS.FOLLOW:
                        newCamera = new CameraFollow(arguments);
                        break;
                    case CAMERA_COMMANDS.DISTANCE:
                        newCamera = new CameraDistance(arguments);
                        break;
                    case CAMERA_COMMANDS.FADE:
                        newCamera = new CameraFade(arguments);
                        break;
                    case CAMERA_COMMANDS.SHAKE:
                        newCamera = new CameraShake(arguments);
                        break;
                    case CAMERA_COMMANDS.PATH:
                        newCamera = new CameraPath(arguments);
                        break;
                    default:
                        newCamera = new Camera(arguments);
                        break;
                }
            }
            else
            {
                throw new Exception("Camera function's first argument has to be camera command identifier");
            }

            return newCamera;
        }
    }

    public class CameraDisable : Camera
    {
        public CameraDisable(List<Node> arguments)
            : base(arguments)
        {
            if (arguments.Count != 1 || !Helper.ValidateArgumentEnumValue(arguments[0], enumName: nameof(CAMERA_COMMANDS), value: (float)CAMERA_COMMANDS.DISABLE))
            {
                throw new Exception("Wrong arguments for camera disable function");
            }
        }
    }

    public class CameraEnable : Camera
    {
        public CameraEnable(List<Node> arguments)
            : base(arguments)
        {
            if (arguments.Count != 1 || !Helper.ValidateArgumentEnumValue(arguments[0], enumName: nameof(CAMERA_COMMANDS), value: (float)CAMERA_COMMANDS.ENABLE))
            {
                throw new Exception("Wrong arguments for camera enable function");
            }
        }
    }

    public class CameraPan : Camera
    {
        public CameraPan(List<Node> arguments)
            : base(arguments)
        {
            if (arguments.Count != 4)
            {
                throw new Exception("Wrong number of arguments for camera pan function");
            }

            if (!Helper.ValidateArgumentEnumValue(arguments[0], enumName: nameof(CAMERA_COMMANDS), value: (float)CAMERA_COMMANDS.PAN))
            {
                throw new Exception("Camera command is not pan");
            }

            if (!(arguments[1] is VectorValue) && !(arguments[1] is FunctionNode))
            {
                throw new Exception("Camera pan command second argument is not Vector or Function");
            }

            if (!(arguments[2] is VectorValue) && !(arguments[2] is FunctionNode))
            {
                throw new Exception("Camera pan command third argument is not Vector or Function");
            }

            if (arguments[3] is FloatValue)
            {
                var floatValue = arguments[3] as FloatValue;
                arguments.Insert(3, new IntegerValue((Int32)floatValue.Float));
                arguments.RemoveAt(4);
            }
            if (!(arguments[3] is IntegerValue) && !(arguments[3] is FunctionNode))
            {
                throw new Exception("Camera pan command fourth argument is not Int or Function");
            }
        }
    }

    public class CameraZoom : Camera
    {
        public CameraZoom(List<Node> arguments)
            : base(arguments)
        {
            if (arguments.Count != 3)
            {
                throw new Exception("Wrong number of arguments for camera zoom function");
            }

            if (!Helper.ValidateArgumentEnumValue(arguments[0], enumName: nameof(CAMERA_COMMANDS), value: (float)CAMERA_COMMANDS.ZOOM))
            {
                throw new Exception("Camera command is not zoom");
            }

            if (!(arguments[1] is FloatValue) && !(arguments[1] is FunctionNode))
            {
                throw new Exception("Camera zoom command second argument is not Float or Function");
            }

            if (arguments[2] is FloatValue)
            {
                var floatValue = arguments[2] as FloatValue;
                arguments.Insert(2, new IntegerValue((Int32)floatValue.Float));
                arguments.RemoveAt(3);
            }
            if (!(arguments[2] is IntegerValue) && !(arguments[2] is FunctionNode))
            {
                throw new Exception("Camera zoom command third argument is not Int or Function");
            }
        }
    }

    public class CameraMove : Camera
    {
        public CameraMove(List<Node> arguments)
            : base(arguments)
        {
            if (arguments.Count != 3)
            {
                throw new Exception("Wrong number of arguments for camera move function");
            }

            if (!Helper.ValidateArgumentEnumValue(arguments[0], enumName: nameof(CAMERA_COMMANDS), value: (float)CAMERA_COMMANDS.MOVE))
            {
                throw new Exception("Camera command is not move");
            }

            if (!(arguments[1] is VectorValue) && !(arguments[1] is FunctionNode))
            {
                throw new Exception("Camera move command second argument is not Vector or Function");
            }

            if (arguments[2] is FloatValue)
            {
                var floatValue = arguments[2] as FloatValue;
                arguments.Insert(2, new IntegerValue((Int32)floatValue.Float));
                arguments.RemoveAt(3);
            }
            if (!(arguments[2] is IntegerValue) && !(arguments[2] is FunctionNode))
            {
                throw new Exception("Camera move command third argument is not Int or Function");
            }
        }
    }

    public class CameraRoll : Camera
    {
        public CameraRoll(List<Node> arguments)
            : base(arguments)
        {
            if (arguments.Count != 3)
            {
                throw new Exception("Wrong number of arguments for camera roll function");
            }

            if (!Helper.ValidateArgumentEnumValue(arguments[0], enumName: nameof(CAMERA_COMMANDS), value: (float)CAMERA_COMMANDS.ROLL))
            {
                throw new Exception("Camera command is not roll");
            }

            if (!(arguments[1] is FloatValue) && !(arguments[1] is FunctionNode))
            {
                throw new Exception("Camera roll command second argument is not Float or Function");
            }

            if (arguments[2] is FloatValue)
            {
                var floatValue = arguments[2] as FloatValue;
                arguments.Insert(2, new IntegerValue((Int32)floatValue.Float));
                arguments.RemoveAt(3);
            }
            if (!(arguments[2] is IntegerValue) && !(arguments[2] is FunctionNode))
            {
                throw new Exception("Camera roll command third argument is not Int or Function");
            }
        }
    }

    public class CameraTrack : Camera
    {
        public CameraTrack(List<Node> arguments)
            : base(arguments)
        {
            if (arguments.Count != 4)
            {
                throw new Exception("Wrong number of arguments for camera track function");
            }

            if (!Helper.ValidateArgumentEnumValue(arguments[0], enumName: nameof(CAMERA_COMMANDS), value: (float)CAMERA_COMMANDS.TRACK))
            {
                throw new Exception("Camera command is not track");
            }

            if (!(arguments[1] is StringValue) && !(arguments[1] is FunctionNode))
            {
                throw new Exception("Camera track command second argument is not String or Function");
            }

            if (!(arguments[2] is FloatValue) && !(arguments[2] is FunctionNode))
            {
                throw new Exception("Camera track command third argument is not Float or Function");
            }

            if (arguments[3] is FloatValue)
            {
                var floatValue = arguments[3] as FloatValue;
                arguments.Insert(3, new IntegerValue((Int32)floatValue.Float));
                arguments.RemoveAt(4);
            }
            if (!(arguments[3] is IntegerValue) && !(arguments[3] is FunctionNode))
            {
                throw new Exception("Camera track command fourth argument is not Int or Function");
            }
        }
    }

    public class CameraFollow : Camera
    {
        public CameraFollow(List<Node> arguments)
            : base(arguments)
        {
            if (arguments.Count != 4)
            {
                throw new Exception("Wrong number of arguments for camera follow function");
            }

            if (!Helper.ValidateArgumentEnumValue(arguments[0], enumName: nameof(CAMERA_COMMANDS), value: (float)CAMERA_COMMANDS.FOLLOW))
            {
                throw new Exception("Camera command is not follow");
            }

            if (!(arguments[1] is StringValue) && !(arguments[1] is FunctionNode))
            {
                throw new Exception("Camera follow command second argument is not String or Function");
            }

            if (!(arguments[2] is FloatValue) && !(arguments[2] is FunctionNode))
            {
                throw new Exception("Camera follow command third argument is not Float or Function");
            }

            if (arguments[3] is FloatValue)
            {
                var floatValue = arguments[3] as FloatValue;
                arguments.Insert(3, new IntegerValue((Int32)floatValue.Float));
                arguments.RemoveAt(4);
            }
            if (!(arguments[3] is IntegerValue) && !(arguments[3] is FunctionNode))
            {
                throw new Exception("Camera follow command fourth argument is not Int or Function");
            }
        }
    }

    public class CameraDistance : Camera
    {
        public CameraDistance(List<Node> arguments)
            : base(arguments)
        {
            if (arguments.Count != 3)
            {
                throw new Exception("Wrong number of arguments for camera distance function");
            }

            if (!Helper.ValidateArgumentEnumValue(arguments[0], enumName: nameof(CAMERA_COMMANDS), value: (float)CAMERA_COMMANDS.DISTANCE))
            {
                throw new Exception("Camera command is not distance");
            }

            if (!(arguments[1] is FloatValue) && !(arguments[1] is FunctionNode))
            {
                throw new Exception("Camera distance command second argument is not Float or Function");
            }

            if (arguments[2] is FloatValue)
            {
                var floatValue = arguments[2] as FloatValue;
                arguments.Insert(2, new IntegerValue((Int32)floatValue.Float));
                arguments.RemoveAt(3);
            }
            if (!(arguments[2] is IntegerValue) && !(arguments[2] is FunctionNode))
            {
                throw new Exception("Camera distance command third argument is not Int or Function");
            }
        }
    }

    public class CameraFade : Camera
    {
        public CameraFade(List<Node> arguments)
            : base(arguments)
        {
            if (arguments.Count != 6)
            {
                throw new Exception("Wrong number of arguments for camera fade function");
            }

            if (!Helper.ValidateArgumentEnumValue(arguments[0], enumName: nameof(CAMERA_COMMANDS), value: (float)CAMERA_COMMANDS.FADE))
            {
                throw new Exception("Camera command is not fade");
            }

            if (!(arguments[1] is VectorValue) && !(arguments[1] is FunctionNode))
            {
                throw new Exception("Camera fade command second argument is not Vector or Function");
            }

            if (!(arguments[2] is FloatValue) && !(arguments[2] is FunctionNode))
            {
                throw new Exception("Camera fade command third argument is not Vector or Function");
            }

            if (!(arguments[3] is VectorValue) && !(arguments[3] is FunctionNode))
            {
                throw new Exception("Camera fade command fourth argument is not Vector or Function");
            }

            if (!(arguments[4] is FloatValue) && !(arguments[4] is FunctionNode))
            {
                throw new Exception("Camera fade command fifth argument is not Vector or Function");
            }

            if (arguments[5] is FloatValue)
            {
                var floatValue = arguments[5] as FloatValue;
                arguments.Insert(5, new IntegerValue((Int32)floatValue.Float));
                arguments.RemoveAt(6);
            }
            if (!(arguments[5] is IntegerValue) && !(arguments[5] is FunctionNode))
            {
                throw new Exception("Camera fade command sixth argument is not Int or Function");
            }
        }
    }

    public class CameraShake : Camera
    {
        public CameraShake(List<Node> arguments)
            : base(arguments)
        {
            if (arguments.Count != 3)
            {
                throw new Exception("Wrong number of arguments for camera shake function");
            }

            if (!Helper.ValidateArgumentEnumValue(arguments[0], enumName: nameof(CAMERA_COMMANDS), value: (float)CAMERA_COMMANDS.SHAKE))
            {
                throw new Exception("Camera command is not shake");
            }

            if (!(arguments[1] is FloatValue) && !(arguments[1] is FunctionNode))
            {
                throw new Exception("Camera shake command second argument is not Float or Function");
            }

            if (arguments[2] is FloatValue)
            {
                var floatValue = arguments[2] as FloatValue;
                arguments.Insert(2, new IntegerValue((Int32)floatValue.Float));
                arguments.RemoveAt(3);
            }
            if (!(arguments[2] is IntegerValue) && !(arguments[2] is FunctionNode))
            {
                throw new Exception("Camera shake command third argument is not Int or Function");
            }
        }
    }

    public class CameraPath : Camera
    {
        public CameraPath(List<Node> arguments)
            : base(arguments)
        {
            if (arguments.Count != 2)
            {
                throw new Exception("Wrong number of arguments for camera path function");
            }

            if (!Helper.ValidateArgumentEnumValue(arguments[0], enumName: nameof(CAMERA_COMMANDS), value: (float)CAMERA_COMMANDS.PATH))
            {
                throw new Exception("Camera command is not path");
            }

            if (!(arguments[1] is StringValue) && !(arguments[1] is FunctionNode))
            {
                throw new Exception("Camera path command second argument is not String or Function");
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
