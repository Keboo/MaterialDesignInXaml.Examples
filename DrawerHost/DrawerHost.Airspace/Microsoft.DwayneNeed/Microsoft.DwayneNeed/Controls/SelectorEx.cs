using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Collections.Specialized;

namespace Microsoft.DwayneNeed.Controls
{
    /// <summary>
    ///     A simple generic version of ItemsControl that overrides the
    ///     virtual methods to support items of type T.
    /// </summary>
    public class SelectorEx<TContainer> : Selector
        where TContainer : DependencyObject, new()
    {
        public SelectorEx()
        {
            ((INotifyCollectionChanged)Items).CollectionChanged += new NotifyCollectionChangedEventHandler(OnItemsCollectionChanged);
        }

        /// <summary>
        ///     Determines if the item needs to be wrapped in a container.
        /// </summary>
        /// <remarks>
        ///     This is determined by whether or not the item is an instance
        ///     of the container type.  This virtual method is sealed.
        /// </remarks>
        protected sealed override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TContainer;
        }

        /// <summary>
        ///     Provides an empty container to be used to wrap an item.
        /// </summary>
        /// <remarks>
        ///     The container is a new instance of the container type.  This
        ///     virtual method is sealed.
        /// </remarks>
        protected sealed override DependencyObject GetContainerForItemOverride()
        {
            TContainer container = new TContainer();
            OnContainerAdded(container);

            return container;
        }

        /// <summary>
        ///     Returns the immediate container from an arbitrary element.
        /// </summary>
        /// <remarks>
        ///     This is a new strongly typed version of the base
        ///     ContainerFromElement method.
        /// </remarks>
        public new TContainer ContainerFromElement(DependencyObject element)
        {
            return (TContainer)base.ContainerFromElement(element);
        }

        /// <summary>
        ///     Clears a container for an item.
        /// </summary>
        /// <remarks>
        ///     In order to implement a strongly typed version of this method,
        ///     this virtual is sealed and it delegates to a new strongly
        ///     typed virtual.
        /// </remarks>
        protected sealed override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            TContainer container = (TContainer)element;
            ClearContainerForItemOverride(container, item);
            OnContainerRemoved(container);
        }

        /// <summary>
        ///     Clears a container for an item.
        /// </summary>
        /// <remarks>
        ///     This is the new strongly typed virtual.  It calls the original
        ///     weakly typed base implementation to preserve functionality.
        /// </remarks>
        protected virtual void ClearContainerForItemOverride(TContainer container, object item)
        {
            base.ClearContainerForItemOverride(container, item);
        }

        /// <summary>
        ///     Prepares a container for an item.
        /// </summary>
        /// <remarks>
        ///     In order to implement a strongly typed version of this method,
        ///     this virtual is sealed and it delegates to a new strongly
        ///     typed virtual.
        /// </remarks>
        protected sealed override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            PrepareContainerForItemOverride((TContainer)element, item);
        }

        /// <summary>
        ///     Prepares a container for an item.
        /// </summary>
        /// <remarks>
        ///     This is the new strongly typed virtual.  It calls the original
        ///     weakly typed base implementation to preserve functionality.
        /// </remarks>
        protected virtual void PrepareContainerForItemOverride(TContainer container, object item)
        {
            base.PrepareContainerForItemOverride((DependencyObject)container, item);
        }

        /// <summary>
        ///     Determines if the item container style should be applied.
        /// </summary>
        /// <remarks>
        ///     This is the new strongly typed virtual.  It calls the original
        ///     weakly typed base implementation to preserve functionality.
        /// </remarks>
        protected sealed override bool ShouldApplyItemContainerStyle(DependencyObject container, object item)
        {
            return ShouldApplyItemContainerStyle((TContainer)container, item);
        }

        /// <summary>
        ///     Determines if the item container style should be applied.
        /// </summary>
        /// <remarks>
        ///     This is the new strongly typed virtual.  It calls the original
        ///     weakly typed base implementation to preserve functionality.
        /// </remarks>
        protected virtual bool ShouldApplyItemContainerStyle(TContainer container, object item)
        {
            return base.ShouldApplyItemContainerStyle(container, item);
        }

        /// <summary>
        ///     Called when the containers are reset.
        /// </summary>
        protected virtual void OnContainersReset()
        {
        }

        /// <summary>
        ///     Called either when a container is generated or when an item
        ///     that is its own container is added.
        /// </summary>
        protected virtual void OnContainerAdded(TContainer container)
        {
        }

        /// <summary>
        ///     Called either when a generated container is removed or when
        ///     an item that is its own container is removed.
        /// </summary>
        protected virtual void OnContainerRemoved(TContainer container)
        {
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    {
                        OnContainersReset();

                        foreach (object item in Items)
                        {
                            if (IsItemItsOwnContainerOverride(item))
                            {
                                OnContainerAdded((TContainer)item);
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (object item in e.NewItems)
                        {
                            if (IsItemItsOwnContainerOverride(item))
                            {
                                OnContainerAdded((TContainer)item);
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    {
                        foreach (object item in e.OldItems)
                        {
                            if (IsItemItsOwnContainerOverride(item))
                            {
                                OnContainerRemoved((TContainer)item);
                            }
                        }

                        foreach (object item in e.NewItems)
                        {
                            if (IsItemItsOwnContainerOverride(item))
                            {
                                OnContainerAdded((TContainer)item);
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (object item in e.OldItems)
                        {
                            if (IsItemItsOwnContainerOverride(item))
                            {
                                OnContainerRemoved((TContainer)item);
                            }
                        }
                    }
                    break;
            }
        }
    }
}
