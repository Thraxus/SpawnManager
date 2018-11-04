namespace SpawnManager.Utilities
{
	internal class FastQueue<T>
	{
		private readonly T[] _nodes;
		private int _current;
		private int _emptySpot;

		public FastQueue(int size)
		{
			_nodes = new T[size];
			this._current = 0;
			this._emptySpot = 0;
		}

		public void Enqueue(T value)
		{
			_nodes[_emptySpot] = value;
			_emptySpot++;
			if (_emptySpot >= _nodes.Length)
			{
				_emptySpot = 0;
			}
		}
		public T Dequeue()
		{
			int ret = _current;
			_current++;
			if (_current >= _nodes.Length)
			{
				_current = 0;
			}
			return _nodes[ret];
		}

		public T[] GetQueue()
		{
			return _nodes;
		}
	}
}
