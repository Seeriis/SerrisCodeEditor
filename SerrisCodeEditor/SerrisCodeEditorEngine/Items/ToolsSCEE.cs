using System;

namespace SerrisCodeEditorEngine.Items
{

    public struct PositionSCEE
    {
        public int row { get; set; }
        public int column { get; set; }
    }

    public struct RangeSCEE
    {
        public int from_row { get; set; }
        public int from_column { get; set; }
        public int to_row { get; set; }
        public int to_column { get; set; }
    }

    public class EventSCEE : EventArgs
    {
        public string message { get; set; }
    }

}
