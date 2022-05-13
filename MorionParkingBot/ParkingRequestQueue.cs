using System.Collections.Concurrent;

namespace MorionParkingBot;

public class ParkingRequestQueue
{
	private readonly ConcurrentQueue<long> _usersInQueue = new ();

	public ParkingRequestQueue()
	{
		
	}

	public long GetLastUserInQueue()
	{
		_usersInQueue.TryPeek(out var userFromQueue);

		return userFromQueue;
	}

	public bool AddInQueue(long telegramUserId)
	{
		if (_usersInQueue.Contains(telegramUserId))
			return false;
		
		_usersInQueue.Enqueue(telegramUserId);

		return true;
	}

	public bool RemoveFromQueue()
	{
		return _usersInQueue.TryDequeue(out var userFromQueue);
	}
}