using AntMe.SharedComponents.AntVideo.Block;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace AntMe.SharedComponents.AntVideo
{
    /// <summary>
    /// Class, to manage all serialize tasks.
    /// </summary>
    internal sealed class Serializer : IDisposable
    {
        private const string HELLOMESSAGE = "AntVI-VideoStream";

        private readonly Stream stream;
        private readonly BinaryWriter writer;
        private readonly BinaryReader reader;
        private readonly byte[] version = new byte[] { 1, 0, 0, 0 };

        public Serializer(Stream stream, bool input, bool output)
        {
            this.stream = stream;

            if (input)
                reader = new BinaryReader(stream, Encoding.ASCII);

            if (output)
                writer = new BinaryWriter(stream, Encoding.ASCII);
        }

        /// <summary>
        /// Reads the next block out of stream.
        /// </summary>
        /// <param name="block">the found block</param>
        /// <returns>type of found block</returns>
        public BlockType Read(out ISerializable block)
        {
            BlockType blockType = (BlockType)stream.ReadByte();
            block = null;

            switch (blockType)
            {
                case BlockType.Ant:
                    block = new Ant(this);
                    break;
                case BlockType.Anthill:
                    block = new Anthill(this);
                    break;
                case BlockType.AnthillLost:
                    throw new InvalidOperationException(string.Format(Resource.AntvideoSerializerInvalidBlockType, blockType));
                case BlockType.AnthillUpdate:
                    throw new InvalidOperationException(string.Format(Resource.AntvideoSerializerInvalidBlockType, blockType));
                case BlockType.AntLost:
                    block = new Lost(this);
                    break;
                case BlockType.AntUpdate:
                    block = new AntUpdate(this);
                    break;
                case BlockType.Bug:
                    block = new Bug(this);
                    break;
                case BlockType.BugLost:
                    block = new Lost(this);
                    break;
                case BlockType.BugUpdate:
                    block = new BugUpdate(this);
                    break;
                case BlockType.Team:
                    block = new Team(this);
                    break;
                case BlockType.TeamLost:
                    throw new InvalidOperationException(string.Format(Resource.AntvideoSerializerInvalidBlockType, blockType));
                case BlockType.TeamUpdate:
                    throw new InvalidOperationException(string.Format(Resource.AntvideoSerializerInvalidBlockType, blockType));
                case BlockType.Colony:
                    block = new Colony(this);
                    break;
                case BlockType.ColonyLost:
                    throw new InvalidOperationException(string.Format(Resource.AntvideoSerializerInvalidBlockType, blockType));
                case BlockType.ColonyUpdate:
                    block = new ColonyUpdate(this);
                    break;
                case BlockType.Frame:
                    block = new Frame(this);
                    break;
                case BlockType.StreamEnd:
                    break;
                case BlockType.FrameLost:
                    throw new InvalidOperationException(string.Format(Resource.AntvideoSerializerInvalidBlockType, blockType));
                case BlockType.FrameUpdate:
                    block = new FrameUpdate(this);
                    break;
                case BlockType.Fruit:
                    block = new Fruit(this);
                    break;
                case BlockType.FruitLost:
                    block = new Lost(this);
                    break;
                case BlockType.FruitUpdate:
                    block = new FruitUpdate(this);
                    break;
                case BlockType.Marker:
                    block = new Marker(this);
                    break;
                case BlockType.MarkerLost:
                    block = new Lost(this);
                    break;
                case BlockType.MarkerUpdate:
                    block = new MarkerUpdate(this);
                    break;
                case BlockType.Caste:
                    block = new Caste(this);
                    break;
                case BlockType.CasteLost:
                    throw new InvalidOperationException(string.Format(Resource.AntvideoSerializerInvalidBlockType, blockType));
                case BlockType.CasteUpdate:
                    throw new InvalidOperationException(string.Format(Resource.AntvideoSerializerInvalidBlockType, blockType));
                case BlockType.Sugar:
                    block = new Sugar(this);
                    break;
                case BlockType.SugarLost:
                    block = new Lost(this);
                    break;
                case BlockType.SugarUpdate:
                    block = new SugarUpdate(this);
                    break;
                case BlockType.FrameStart:
                    break;
                case BlockType.FrameEnd:
                    break;
                default:
                    throw new InvalidOperationException(string.Format(Resource.AntvideoSerializerInvalidBlockType, blockType));
            }

            return blockType;
        }

        /// <summary>
        /// Writes the hello-header to stream.
        /// </summary>
        public void WriteHello()
        {
            byte[] output = ASCIIEncoding.ASCII.GetBytes(HELLOMESSAGE);
            stream.Write(output, 0, output.Length);
            stream.WriteByte(version[0]);
            stream.WriteByte(version[1]);
            stream.WriteByte(version[2]);
            stream.WriteByte(version[3]);
        }

        /// <summary>
        /// Reads hello-header out of stream.
        /// </summary>
        public void ReadHello()
        {

            // Compare hello-message
            byte[] compare = ASCIIEncoding.ASCII.GetBytes(HELLOMESSAGE);
            stream.Read(compare, 0, compare.Length);
            if (ASCIIEncoding.ASCII.GetString(compare) != HELLOMESSAGE)
            {
                throw new NotSupportedException(Resource.AntvideoSerializerWrongHeader);
            }

            // Compare version number
            byte[] vers = new byte[4];
            stream.Read(vers, 0, 4);
            if (vers[0] != version[0] ||
                vers[1] != version[1] ||
                vers[2] != version[2] ||
                vers[3] != version[3])
            {
                throw new NotSupportedException(
                    string.Format(
                    CultureInfo.CurrentCulture,
                    Resource.AntvideoSerializerWrongProtocolVersion, vers[0], vers[1], vers[2], vers[3]));
            }

            if ((BlockType)stream.ReadByte() != BlockType.StreamStart)
            {
                throw new NotSupportedException(Resource.AntvideoSerializerWrongHeader);
            }
        }

        /// <summary>
        /// Writes a given block to stream.
        /// </summary>
        /// <param name="blocktype">type of given block</param>
        /// <param name="block">block, to write</param>
        public void Write(BlockType blocktype, ISerializable block)
        {
            stream.WriteByte((byte)blocktype);
            if (block != null)
            {
                block.Serialize(this);
            }
        }

        /// <summary>
        /// Writes a given block to stream.
        /// </summary>
        /// <param name="blocktype">type of block</param>
        public void Write(BlockType blocktype)
        {
            Write(blocktype, null);
        }

        /// <summary>
        /// Reads the following byte without affect the stream position.
        /// </summary>
        /// <returns>next byte</returns>
        public int Peek()
        {
            int output = stream.ReadByte();
            stream.Seek(-1, SeekOrigin.Current);
            return output;
        }

        internal void Flush()
        {
            stream.Flush();
        }

        #region Sendemethoden

        /// <summary>
        /// Sends ushort over the stream.
        /// </summary>
        /// <param name="item">ushort</param>
        public void SendUshort(ushort item)
        {
            writer.Write(item);
        }

        /// <summary>
        /// Sends a short.
        /// </summary>
        /// <param name="item">short</param>
        public void SendShort(short item)
        {
            writer.Write(item);
        }

        /// <summary>
        /// Sends byte over the stream.
        /// </summary>
        /// <param name="item">sbyte</param>
        public void SendSByte(sbyte item)
        {
            writer.Write(item);
        }

        /// <summary>
        /// Sends integer over the stream.
        /// </summary>
        /// <param name="item">integer</param>
        public void SendInt(int item)
        {
            writer.Write(item);
        }

        public void SendLong(long item)
        {
            writer.Write(item);
        }

        /// <summary>
        /// Sends string over the stream.
        /// </summary>
        /// <param name="item">string</param>
        public void SendString(string item)
        {
            writer.Write(item);
        }

        public void SendDateTime(DateTime item)
        {
            SendLong(item.Ticks);
        }

        public void SendGuid(Guid guid)
        {
            byte[] output = guid.ToByteArray();
            stream.Write(output, 0, output.Length);
        }

        public void SendByte(byte item)
        {
            writer.Write(item);
        }

        #endregion

        #region Receive methods

        /// <summary>
        /// Reads next ushort from the specified stream.
        /// </summary>
        /// <returns>read ushort</returns>
        public ushort ReadUShort()
        {
            return reader.ReadUInt16();
        }

        public short ReadShort()
        {
            return reader.ReadInt16();
        }

        /// <summary>
        /// Reads next sbyte from the specified stream.
        /// </summary>
        /// <returns>read sbyte</returns>
        public sbyte ReadSByte()
        {
            return reader.ReadSByte();
        }

        /// <summary>
        /// Reads next integer from the specified stream.
        /// </summary>
        /// <returns>read integer</returns>
        public int ReadInt()
        {
            return reader.ReadInt32();
        }

        public long ReadLong()
        {
            return reader.ReadInt64();
        }

        /// <summary>
        /// Reads next string from the specified stream.
        /// </summary>
        /// <returns>read string</returns>
        public string ReadString()
        {
            return reader.ReadString();
        }

        /// <summary>
        /// Reads next datetime from the specified stream.
        /// </summary>
        /// <returns>read DateTime</returns>
        public DateTime ReadDateTime()
        {
            return new DateTime(ReadLong());
        }

        public Guid ReadGuid()
        {
            byte[] input = new byte[16];
            stream.Read(input, 0, 16);
            return new Guid(input);
        }

        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        #endregion

        #region IDisposable Member

        public void Dispose()
        {
            if (writer != null)
            {
                ((IDisposable)writer).Dispose();
            }

            if (reader != null)
            {
                ((IDisposable)reader).Dispose();
            }
        }

        #endregion
    }
}