using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using OscCore.Address;

namespace OscCore.Invoke
{
//    public class OscInvoker<TOscMessage> where TOscMessage : IOscMessage
//    {
//        public delegate void OscMessageEvent(TOscMessage message);
//
//        private readonly ConcurrentDictionary<string, OscEventContainer> literalAddresses = new ConcurrentDictionary<string, OscEventContainer>();
//        private readonly ConcurrentDictionary<string, OscEventContainer> patternAddresses = new ConcurrentDictionary<string, OscEventContainer>();
//
//        public void Attach(string address, OscMessageEvent @event)
//        {
//            if (@event == null)
//            {
//                throw new ArgumentNullException(nameof(@event));
//            }
//
//            // if the address is a literal then add it to the literal lookup
//            if (OscAddress.IsValidAddressLiteral(address))
//            {
//                OscEventContainer container = literalAddresses.GetOrAdd(address, func => new OscEventContainer(new OscAddress(address)));
//
//                // attach the event
//                container.Event += @event;
//            }
//            // if the address is a pattern add it to the pattern lookup
//            else if (OscAddress.IsValidAddressPattern(address))
//            {
//                // add it to the lookup
//                OscEventContainer container = patternAddresses.GetOrAdd(address, func => new OscEventContainer(new OscAddress(address)));
//
//                // attach the event
//                container.Event += @event;
//            }
//            else
//            {
//                throw new ArgumentException($"Invalid container address '{address}'", nameof(address));
//            }
//        }
//
//        public bool Contains(OscAddress oscAddress)
//        {
//            return Contains(oscAddress.ToString());
//        }
//
//        public bool Contains(string oscAddress)
//        {
//            return patternAddresses.ContainsKey(oscAddress) || literalAddresses.ContainsKey(oscAddress);
//        }
//
//        public bool ContainsLiteral(string oscAddress)
//        {
//            return literalAddresses.ContainsKey(oscAddress);
//        }
//
//        public bool ContainsPattern(OscAddress oscAddress)
//        {
//            return patternAddresses.ContainsKey(oscAddress.ToString());
//        }
//
//        /// <summary>
//        ///     Detach an event listener
//        /// </summary>
//        /// <param name="address">the address of the container</param>
//        /// <param name="event">the event to remove</param>
//        public void Detach(string address, OscMessageEvent @event)
//        {
//            if (@event == null)
//            {
//                throw new ArgumentNullException(nameof(@event));
//            }
//
//            if (OscAddress.IsValidAddressLiteral(address))
//            {
//                if (literalAddresses.TryGetValue(address, out OscEventContainer container) == false)
//                {
//                    // no container was found so abort
//                    return;
//                }
//
//                // unregiser the event
//                container.Event -= @event;
//
//                // if the container is now empty remove it from the lookup
//                if (container.IsNull)
//                {
//                    literalAddresses.TryRemove(container.Address, out container);
//                }
//            }
//            else if (OscAddress.IsValidAddressPattern(address))
//            {
//                if (patternAddresses.TryGetValue(address, out OscEventContainer container) == false)
//                {
//                    // no container was found so abort
//                    return;
//                }
//
//                // unregiser the event
//                container.Event -= @event;
//
//                // if the container is now empty remove it from the lookup
//                if (container.IsNull)
//                {
//                    patternAddresses.TryRemove(container.Address, out container);
//                }
//            }
//            else
//            {
//                throw new ArgumentException($"Invalid container address '{address}'", nameof(address));
//            }
//        }
//
//        /// <summary>
//        ///     Disposes of any resources and releases all events
//        /// </summary>
//        public void Dispose()
//        {
//            foreach (KeyValuePair<string, OscEventContainer> value in literalAddresses)
//            {
//                value.Value.Clear();
//            }
//
//            literalAddresses.Clear();
//
//            foreach (KeyValuePair<string, OscEventContainer> value in patternAddresses)
//            {
//                value.Value.Clear();
//            }
//
//            patternAddresses.Clear();
//        }
//
//        public IEnumerator<OscAddress> GetEnumerator()
//        {
//            return GetAllAddresses()
//                .GetEnumerator();
//        }
//
//        public void Invoke(TOscMessage message)
//        {
//            bool invoked = false;
//
//            if (OscAddress.IsValidAddressLiteral(message.Address))
//            {
//                if (literalAddresses.TryGetValue(message.Address, out OscEventContainer container))
//                {
//                    container.Invoke(message);
//
//                    invoked = true;
//                }
//            }
//            else
//            {
//                foreach (KeyValuePair<string, OscEventContainer> value in literalAddresses)
//                {
//                    OscAddress oscAddress = value.Value.OscAddress;
//
//                    if (value.Value.OscAddress.Match(value.Key) != true)
//                    {
//                        continue;
//                    }
//
//                    value.Value.Invoke(message);
//
//                    invoked = true;
//                }
//            }
//
//            if (patternAddresses.Count > 0)
//            {
//                OscAddress oscAddress = new OscAddress(message.Address);
//
//                foreach (KeyValuePair<string, OscEventContainer> value in patternAddresses)
//                {
//                    if (oscAddress.Match(value.Key) == false)
//                    {
//                        continue;
//                    }
//
//                    value.Value.Invoke(message);
//                    invoked = true;
//                }
//            }
//
//            if (invoked == false)
//            {
//                UnknownAddress?.Invoke(message);
//            }
//        }
//
//        public void Invoke(IEnumerable<TOscMessage> messages)
//        {
//            foreach (TOscMessage message in messages)
//            {
//                Invoke(message);
//            }
//        }
//
//        public event OscMessageEvent UnknownAddress;
//
//        private List<OscAddress> GetAllAddresses()
//        {
//            List<OscAddress> addresses = new List<OscAddress>();
//
//            addresses.AddRange(patternAddresses.Values.Select(container => container.OscAddress));
//
//            addresses.AddRange(literalAddresses.Values.Select(container => container.OscAddress));
//
//            return addresses;
//        }
//
//        private class OscEventContainer
//        {
//            public readonly string Address;
//            public readonly OscAddress OscAddress;
//
//            public bool IsNull => Event == null;
//
//            public OscEventContainer(OscAddress address)
//            {
//                OscAddress = address;
//
//                Address = address.ToString();
//
//                Event = null;
//            }
//
//            public void Clear()
//            {
//                Event = null;
//            }
//
//            public event OscMessageEvent Event;
//
//            public void Invoke(TOscMessage message)
//            {
//                Event?.Invoke(message);
//            }
//        }
//    }
}