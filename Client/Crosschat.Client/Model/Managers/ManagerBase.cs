using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using SharedSquawk.Client.Model.Helpers;
using SharedSquawk.Client.Model.Proxies;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;

namespace SharedSquawk.Client.Model.Managers
{
    public abstract class ManagerBase
    {
        protected ManagerBase(ConnectionManager connectionManager)
        {
            if (connectionManager != null)
            {
                ConnectionManager = connectionManager;
            }
        }

        protected ConnectionManager ConnectionManager { get; private set; }

        protected virtual void OnUnknownDtoReceived(BaseDto dto) { }

        protected virtual Task<ResponseBase> OnRequest(RequestBase request) { return Task.FromResult<ResponseBase>(null); }

        protected void ToEntityAndAddToList<TEntity, TListItem, TDto>(BaseDto dto, ObservableCollection<TListItem> list, Func<TDto, BaseDto> dtoSelector = null, bool addToEnd = true)
            where TDto : BaseDto
            where TEntity : TListItem
        {
            if (!(dto is TDto))
                return;

            if (dtoSelector == null)
                dtoSelector = d => d;

            var entity = ToEntity<TEntity>(dtoSelector((TDto)dto));

            if (addToEnd)
            {
                list.Add(entity);
            }
            else
            {
                list.Insert(0, entity);
            }
        }

        protected void UpdatePropertiesForList<T>(ObservableCollection<T> list, Func<T, bool> selector, PropertyValuePair[] properties)
        {
            list.ForEach(item =>
                {
                    if (selector(item))
                        UpdateProperties(item, properties);
                });
        }

        protected void UpdateProperties<T>(T instance, PropertyValuePair[] properties)
        {
            var type = instance.GetType().GetTypeInfo();
            foreach (var propertyValuePair in properties)
            {
                var propertyType = type.GetDeclaredProperty(propertyValuePair.Property);
                propertyType.SetValue(instance, Convert.ChangeType(propertyValuePair.Value, propertyType.PropertyType));
            }
        }

        protected void RemoveEntityFromList<TEntity>(ObservableCollection<TEntity> list,
            Func<TEntity, bool> selector)
        {
            try
            {
                list.RemoveAll(selector);
            }
            catch (Exception)
            {
                //log
            }
        }

        protected TEntity ToEntity<TEntity>(BaseDto dto)
        {
            var entity = Activator.CreateInstance<TEntity>();
            AutoMapper.CopyPropertyValues(dto, entity);
            return entity;
        }
    }
}
