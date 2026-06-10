using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Services
{
    public class RoomService
    {
        public async Task<List<RoomData>> GetAllRoomsAsync()
        {
            using (var context = new AppDbContext())
            {
                return await context.Rooms.Select(r => new RoomData
                {
                    RoomId = r.RoomId,
                    RoomName = r.RoomName,
                    RoomType = r.RoomType,
                    Building = r.Building,
                    Capacity = r.Capacity,
                    Status = r.Status
                }).ToListAsync();
            }
        }

        public async Task SaveRoomAsync(RoomData room)
        {
            using (var context = new AppDbContext())
            {
                if (room.RoomId == 0)
                {
                    context.Rooms.Add(new Room
                    {
                        RoomName = room.RoomName,
                        RoomType = room.RoomType,
                        Building = room.Building,
                        Capacity = room.Capacity,
                        Status = room.Status
                    });
                }
                else
                {
                    var dbRoom = await context.Rooms.FindAsync(room.RoomId);
                    if (dbRoom != null)
                    {
                        dbRoom.RoomName = room.RoomName;
                        dbRoom.RoomType = room.RoomType;
                        dbRoom.Building = room.Building;
                        dbRoom.Capacity = room.Capacity;
                        dbRoom.Status = room.Status;
                    }
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
