START ../../PacketGenerator/bin/PacketGenerator.exe ../../PacketGenerator/PDL.xml
XCOPY /Y GenPacket.cs "../../DummyClient/Packet"
XCOPY /Y GenPacket.cs "../../Client/Assets/Scripts/Packet"
XCOPY /Y GenPacket.cs "../../Server/Packet"

XCOPY /Y ClientPacketManager.cs "../../DummyClient/Packet"
XCOPY /Y ClientPacketManager.cs "../../Client/Assets/Scripts/Packet"
XCOPY /Y ServerPacketManager.cs "../../Server/Packet"
