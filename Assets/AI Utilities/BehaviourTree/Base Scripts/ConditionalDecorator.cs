namespace BehaviourTree
{
    public class ConditionalDecorator : Node
    {
        private readonly System.Func<NODESTATE> condition;

        public ConditionalDecorator(System.Func<NODESTATE> condition)
        {
            this.condition = condition;
        }

        public override NODESTATE Evaluate()
        {
            return condition();
        }
    }
}
