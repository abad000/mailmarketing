using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace MailBC.DataStore.Infrastructure.Extensions
{
    /// <summary>
    /// Check Matt Warren's Sample
    /// http://blogs.msdn.com/mattwar/archive/2007/07/31/linq-building-an-iqueryable-provider-part-ii.aspx
    /// </summary>
    public abstract class ExpressionVisitor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual Expression Visit(Expression expression)
        {
            if (expression == null) return null;

            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.AddChecked:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.AndAssign:
                case ExpressionType.ArrayLength:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Assign:
                case ExpressionType.Block:
                case ExpressionType.Call:
                    return VisitCall((MethodCallExpression)expression);
                case ExpressionType.Coalesce:
                case ExpressionType.Conditional:
                    return VisitConditional((ConditionalExpression)expression);
                case ExpressionType.Constant:
                    return VisitConstant((ConstantExpression)expression);
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.DebugInfo:
                case ExpressionType.Decrement:
                case ExpressionType.Default:
                case ExpressionType.Divide:
                case ExpressionType.DivideAssign:
                case ExpressionType.Dynamic:
                case ExpressionType.Equal:
                case ExpressionType.ExclusiveOr:
                    return VisitBinary((BinaryExpression)expression);
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.Extension:
                case ExpressionType.Goto:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Increment:
                case ExpressionType.Index:
                case ExpressionType.Invoke:
                    return VisitInvoke((InvocationExpression)expression);
                case ExpressionType.IsFalse:
                case ExpressionType.IsTrue:
                case ExpressionType.Lambda:
                    return VisitLambda((LambdaExpression)expression);
                case ExpressionType.Label:
                case ExpressionType.LeftShift:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.ListInit:
                    return VisitListInit((ListInitExpression)expression);
                case ExpressionType.Loop:
                case ExpressionType.MemberAccess:
                    return VisitMemberAccess((MemberExpression)expression);
                case ExpressionType.MemberInit:
                    return VisitMemberInit((MemberInitExpression)expression);
                case ExpressionType.Modulo:
                case ExpressionType.ModuloAssign:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.New:
                    return VisitNew((NewExpression)expression);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return VisitNewArrayBounds((NewArrayExpression)expression);
                case ExpressionType.Not:
                case ExpressionType.NotEqual:
                case ExpressionType.OnesComplement:
                case ExpressionType.Or:
                case ExpressionType.OrAssign:
                case ExpressionType.OrElse:
                case ExpressionType.Parameter:
                    return VisitParameter((ParameterExpression)expression);
                case ExpressionType.Power:
                case ExpressionType.PowerAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.Quote:
                case ExpressionType.RightShift:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.RuntimeVariables:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Switch:
                case ExpressionType.Throw:
                case ExpressionType.Try:
                case ExpressionType.TypeEqual:
                case ExpressionType.TypeIs:
                    return VisitTypeIs((TypeBinaryExpression)expression);
                case ExpressionType.UnaryPlus:
                case ExpressionType.Unbox:
                case ExpressionType.TypeAs:
                    return VisitUnary((UnaryExpression)expression);
                default:
                    throw new UnhandledTypeException(string.Format("Unhandled expression type: '{0}'", expression.NodeType));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binaryExp"></param>
        /// <returns></returns>
        protected virtual Expression VisitBinary(BinaryExpression binaryExp)
        {
            Expression left = Visit(binaryExp.Left);
            Expression right = Visit(binaryExp.Right);
            Expression conversion = Visit(binaryExp.Conversion);

            return left != binaryExp.Left || right != binaryExp.Right || conversion != binaryExp.Conversion
                ? (binaryExp.NodeType == ExpressionType.Coalesce && conversion != null
                        ? Expression.Coalesce(left, right, conversion as LambdaExpression)
                        : Expression.MakeBinary(binaryExp.NodeType, left, right, binaryExp.IsLiftedToNull, binaryExp.Method))
                : binaryExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodCallExp"></param>
        /// <returns></returns>
        protected virtual Expression VisitCall(MethodCallExpression methodCallExp)
        {
            Expression @object = Visit(methodCallExp.Object);
            IEnumerable<Expression> args = VisitExpressionList(methodCallExp.Arguments);

            return @object != methodCallExp.Object || !Equals(args, methodCallExp.Arguments)
                ? Expression.Call(@object, methodCallExp.Method, args)
                : methodCallExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<Expression> list = null;

            for (int i = 0, n = original.Count; i < n; i++)
            {
                Expression p = Visit(original[i]);
                if (list != null)
                {
                    list.Add(p);
                }
                else if (p != original[i])
                {
                    list = new List<Expression>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(p);
                }
            }

            return list != null ? list.AsReadOnly() : original;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionalExp"></param>
        /// <returns></returns>
        protected virtual Expression VisitConditional(ConditionalExpression conditionalExp)
        {
            Expression test = Visit(conditionalExp.Test);
            Expression ifTrue = Visit(conditionalExp.IfTrue);
            Expression ifFalse = Visit(conditionalExp.IfFalse);

            return test != conditionalExp.Test || ifTrue != conditionalExp.IfTrue || ifFalse != conditionalExp.IfFalse
                ? Expression.Condition(test, ifTrue, ifFalse)
                : conditionalExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constantExp"></param>
        /// <returns></returns>
        protected virtual Expression VisitConstant(ConstantExpression constantExp)
        {
            return constantExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocationExp"></param>
        /// <returns></returns>
        protected virtual Expression VisitInvoke(InvocationExpression invocationExp)
        {
            IEnumerable<Expression> args = VisitExpressionList(invocationExp.Arguments);
            Expression expression = Visit(invocationExp.Expression);

            return !Equals(args, invocationExp.Arguments) || expression != invocationExp.Expression
                ? Expression.Invoke(expression, args)
                : invocationExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lambdaExp"></param>
        /// <returns></returns>
        protected virtual Expression VisitLambda(LambdaExpression lambdaExp)
        {
            Expression body = Visit(lambdaExp.Body);

            return body != lambdaExp.Body
                ? Expression.Lambda(lambdaExp.Type, body, lambdaExp.Parameters)
                : lambdaExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listInitExp"></param>
        /// <returns></returns>
        protected virtual Expression VisitListInit(ListInitExpression listInitExp)
        {
            NewExpression newExpression = VisitNew(listInitExp.NewExpression);
            IEnumerable<ElementInit> initializers = VisitElementInitializerList(listInitExp.Initializers);

            return newExpression != listInitExp.NewExpression || !Equals(initializers, listInitExp.Initializers)
                ? Expression.ListInit(newExpression, initializers)
                : listInitExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
        {
            List<ElementInit> list = null;

            for (int i = 0, n = original.Count; i < n; i++)
            {
                ElementInit init = VisitElementInitializer(original[i]);
                if (list != null)
                {
                    list.Add(init);
                }
                else if (init != original[i])
                {
                    list = new List<ElementInit>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(init);
                }
            }

            return list != null ?  list.AsEnumerable() : original;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementInit"></param>
        /// <returns></returns>
        protected virtual ElementInit VisitElementInitializer(ElementInit elementInit)
        {
            ReadOnlyCollection<Expression> arguments = VisitExpressionList(elementInit.Arguments);

            return arguments != elementInit.Arguments 
                ? Expression.ElementInit(elementInit.AddMethod, arguments)
                : elementInit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberExp"></param>
        /// <returns></returns>
        protected virtual Expression VisitMemberAccess(MemberExpression memberExp)
        {
            Expression expression = Visit(memberExp.Expression);

            return expression != memberExp.Expression 
                ? Expression.MakeMemberAccess(expression, memberExp.Member)
                : memberExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberInitExp"></param>
        /// <returns></returns>
        protected virtual Expression VisitMemberInit(MemberInitExpression memberInitExp)
        {
            NewExpression n = VisitNew(memberInitExp.NewExpression);
            IEnumerable<MemberBinding> bindings = VisitBindingList(memberInitExp.Bindings);

            return n != memberInitExp.NewExpression || !Equals(bindings, memberInitExp.Bindings)
                ? Expression.MemberInit(n, bindings)
                : memberInitExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
        {
            List<MemberBinding> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                MemberBinding b = VisitBinding(original[i]);
                if (list != null)
                {
                    list.Add(b);
                }
                else if (b != original[i])
                {
                    list = new List<MemberBinding>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(b);
                }
            }

            return list != null ? list.AsEnumerable() : original;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberBinding"></param>
        /// <returns></returns>
        protected virtual MemberBinding VisitBinding(MemberBinding memberBinding)
        {
            switch (memberBinding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return VisitMemberAssignment((MemberAssignment)memberBinding);
                case MemberBindingType.MemberBinding:
                    return VisitMemberMemberBinding((MemberMemberBinding)memberBinding);
                case MemberBindingType.ListBinding:
                    return VisitMemberListBinding((MemberListBinding)memberBinding);
                default:
                    throw new UnhandledTypeException(string.Format("Unhandled binding type '{0}'", memberBinding.BindingType));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberAssignment"></param>
        /// <returns></returns>
        protected virtual MemberBinding VisitMemberAssignment(MemberAssignment memberAssignment)
        {
            Expression e = Visit(memberAssignment.Expression);

            return e != memberAssignment.Expression
                ? Expression.Bind(memberAssignment.Member, e)
                : memberAssignment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberMemberBinding"></param>
        /// <returns></returns>
        protected virtual MemberBinding VisitMemberMemberBinding(MemberMemberBinding memberMemberBinding)
        {
            IEnumerable<MemberBinding> bindings = VisitBindingList(memberMemberBinding.Bindings);

            return !Equals(bindings, memberMemberBinding.Bindings)
                ? Expression.MemberBind(memberMemberBinding.Member, bindings)
                : memberMemberBinding;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberListBinding"></param>
        /// <returns></returns>
        protected virtual MemberBinding VisitMemberListBinding(MemberListBinding memberListBinding)
        {
            IEnumerable<ElementInit> initializers = VisitElementInitializerList(memberListBinding.Initializers);

            return !Equals(initializers, memberListBinding.Initializers)
                ? Expression.ListBind(memberListBinding.Member, initializers)
                : memberListBinding;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newExp"></param>
        /// <returns></returns>
        protected virtual NewExpression VisitNew(NewExpression newExp)
        {
            IEnumerable<Expression> args = VisitExpressionList(newExp.Arguments);

            return !Equals(args, newExp.Arguments)
                ? (newExp.Members != null
                        ? Expression.New(newExp.Constructor, args, newExp.Members)
                        : Expression.New(newExp.Constructor, args))
                : newExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newArrayExp"></param>
        /// <returns></returns>
        protected virtual Expression VisitNewArrayBounds(NewArrayExpression newArrayExp)
        {
            IEnumerable<Expression> exprs = VisitExpressionList(newArrayExp.Expressions);

            return !Equals(exprs, newArrayExp.Expressions)
                ? (newArrayExp.NodeType == ExpressionType.NewArrayInit
                        ? Expression.NewArrayInit(newArrayExp.Type.GetElementType(), exprs)
                        : Expression.NewArrayBounds(newArrayExp.Type.GetElementType(), exprs))
                : newArrayExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramExp"></param>
        /// <returns></returns>
        protected virtual Expression VisitParameter(ParameterExpression paramExp)
        {
            return paramExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeBinaryExp"></param>
        /// <returns></returns>
        protected virtual Expression VisitTypeIs(TypeBinaryExpression typeBinaryExp)
        {
            Expression expression = Visit(typeBinaryExp.Expression);

            return expression != typeBinaryExp.Expression 
                ? Expression.TypeIs(expression, typeBinaryExp.TypeOperand) 
                : typeBinaryExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unaryExp"></param>
        /// <returns></returns>
        protected virtual Expression VisitUnary(UnaryExpression unaryExp)
        {
            Expression operand = Visit(unaryExp.Operand);

            return operand != unaryExp.Operand
                ? Expression.MakeUnary(unaryExp.NodeType, operand, unaryExp.Type, unaryExp.Method)
                : unaryExp;
        }
    }
}