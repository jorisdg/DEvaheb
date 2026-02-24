using System;
using System.Collections.Generic;
using System.Linq;
using DEvahebLib.Enums;
using DEvahebLib.Nodes;

namespace DEvahebLib.Visitors
{
    public class TransformNodes : Visitor
    {
        public static void Transform(List<Node> nodes)
        {
            var visitor = new TransformNodes();
            visitor.Visit(nodes);
        }

        public static void Transform(Node node)
        {
            var visitor = new TransformNodes();
            visitor.Visit(node);
        }

        public override void VisitFunctionNode(FunctionNode node)
        {
            base.VisitFunctionNode(node);
            TransformNode(node);
        }

        public override void VisitBlockNode(BlockNode node)
        {
            base.VisitBlockNode(node);
            TransformNode(node);
        }

        private void TransformNode(FunctionNode node)
        {
            if (node is Tag tag)
                TransformTag(tag);
            else if (node is Get get)
                TransformGet(get);
            else if (node is Loop loop)
                TransformLoop(loop);
            else if (node is Camera camera)
                TransformCamera(camera);
        }

        private void TransformTag(Tag tag)
        {
            var args = tag.Arguments.ToList();
            if (args.Count > 1 && args[1] is IdentifierValue id)
            {
                var table = EnumTableFloat.FromEnum(typeof(TagType));
                if (table.HasEnum(id.IdentifierName))
                {
                    var resolved = new FloatValue(table.GetValue(id.IdentifierName));
                    tag.SetArg(1, EnumValue.CreateOrPassThrough(resolved, typeof(TagType)));
                }
            }
        }

        private void TransformGet(Get get)
        {
            var args = get.Arguments.ToList();
            if (args.Count > 0 && args[0] is IdentifierValue id)
            {
                var table = EnumTableFloat.FromEnum(typeof(DECLARE_TYPE));
                if (table.HasEnum(id.IdentifierName))
                {
                    var resolved = new FloatValue(table.GetValue(id.IdentifierName));
                    get.SetArg(0, EnumValue.CreateOrPassThrough(resolved, typeof(DECLARE_TYPE)));
                }
            }
        }

        private void TransformLoop(Loop loop)
        {
            var args = loop.Arguments.ToList();
            if (args.Count > 0 && args[0] is FloatValue floatValue)
            {
                loop.SetArg(0, new IntegerValue((Int32)floatValue.Float));
            }
        }

        private void TransformCamera(Camera camera)
        {
            var cmd = camera.GetCommand();
            if (cmd == null) return;

            int durationIndex = -1;
            switch (cmd.Value)
            {
                case CAMERA_COMMANDS.ZOOM:
                case CAMERA_COMMANDS.MOVE:
                case CAMERA_COMMANDS.ROLL:
                case CAMERA_COMMANDS.DISTANCE:
                case CAMERA_COMMANDS.SHAKE:
                    durationIndex = 2;
                    break;
                case CAMERA_COMMANDS.PAN:
                case CAMERA_COMMANDS.TRACK:
                case CAMERA_COMMANDS.FOLLOW:
                    durationIndex = 3;
                    break;
                case CAMERA_COMMANDS.FADE:
                    durationIndex = 5;
                    break;
            }

            var args = camera.Arguments.ToList();
            if (durationIndex >= 0 && durationIndex < args.Count && args[durationIndex] is FloatValue floatValue)
            {
                camera.SetArg(durationIndex, new IntegerValue((Int32)floatValue.Float));
            }
        }
    }
}
