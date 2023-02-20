using Grpc.Core;
using MagicOnion.Serialization;
using MessagePack;
using MessagePack.Resolvers;
using System.Buffers;
using System.Reflection;

namespace Api.MagicOnion
{
    public class MagicOnionMsgPckSerializerProvider : IMagicOnionSerializerProvider
    {
        private MagicOnionMessagePackSerializer serializer = new MagicOnionMessagePackSerializer();
        public IMagicOnionSerializer Create(MethodType methodType, MethodInfo methodInfo)
        {
            return serializer;
        }
    }
    public class MagicOnionMessagePackSerializer : IMagicOnionSerializer
    {
        MessagePackSerializerOptions options = StandardResolver.Options;
        public void Serialize<T>(IBufferWriter<byte> writer, in T value)
        {

            MessagePackSerializer.Serialize<T>(writer, value, options);
        }

        public T Deserialize<T>(in ReadOnlySequence<byte> bytes)
        {
            return MessagePackSerializer.Deserialize<T>(bytes, options);
        }
    }
}
