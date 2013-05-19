using System.Collections.Generic;
using System.Linq.Expressions;

namespace MailBC.DataStore.Infrastructure.Extensions
{
    /// <summary>
    /// http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx
    /// </summary>
    public class ParameterRebinder : MailBC.DataStore.Infrastructure.Extensions.ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

        /// <summary>
        /// Intializes a new instance of the <see cref="ParameterRebinder" /> class
        /// </summary>
        /// <param name="map">Dictionary map.</param>
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression expression)
        {
            return new ParameterRebinder(map).Visit(expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramExp"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression paramExp)
        {
            ParameterExpression replacement;

            if (_map.TryGetValue(paramExp, out replacement))
            {
                paramExp = replacement;
            }

            return base.VisitParameter(paramExp);
        }
    }
}